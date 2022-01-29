using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class VotingManager : MonoBehaviourPun
{
    public static VotingManager Instance;
    [HideInInspector] public PhotonView DeadBodyInProximity;
    public List<int> _reportedDeadBodiesList = new List<int>();
    [SerializeField] private GameObject _emergencyMeetingWindow;
    [SerializeField] private VotePlayerItem _votePlayerItemPrefab;
    [SerializeField] private Transform _votePlayerItemContainer;
    public List<VotePlayerItem> _votePlyaerItemList = new List<VotePlayerItem>();
    [SerializeField] private Button _skipVoteBtn;
    [HideInInspector] private bool HasAlreadyVoted;
    private List<VotePlayerItem> _votePlayerItemList = new List<VotePlayerItem>();
    private List<int> _playerThatVotedList = new List<int>();
    private List<int> _playersThatHaveBeenVoteList = new List<int>();
    private List<int> _playerThatHaveBeenKickedOutList = new List<int>();
    [SerializeField] private GameObject _kickPlayerWindow;

    [SerializeField] private GameObject _kickedPlayerWindow;
    [SerializeField] private Text _kickPlayerText;
    [SerializeField] private Network _network;
    [SerializeField] public GameObject ChooseRoleWindow;
    [SerializeField] public GameObject ChooseRoleContainer;

    [SerializeField] private GameObject RoleBtn;
    public enum playerAction
    {
        Vote,
        ChooseRole,
        Action
    }
    public playerAction CurrentAction = playerAction.Vote;
    public int currentTarget;
    public Role LocalPlayer;
    public MasterClient.Role CurrentChooseRole;
    public List<MasterClient.Role> allrole = new List<MasterClient.Role>(){
        MasterClient.Role.Process,
        MasterClient.Role.Scanner,
        MasterClient.Role.Deleter,
        MasterClient.Role.Worm,
        MasterClient.Role.Spyware
    };
    public Dictionary<MasterClient.Role, string> allroledict = new Dictionary<MasterClient.Role, string>(){
        {MasterClient.Role.Process,"process"},
        {MasterClient.Role.Scanner,"Scanner"},
        {MasterClient.Role.Deleter,"Deleter"},
        {MasterClient.Role.Worm,"Worm"},
        {MasterClient.Role.Spyware,"Spyware"}
    };
    public List<Button> RoleBtnGroup;
    public UnityEvent onChooseRole;
    public UnityEvent onEndVote;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }


    public bool WasBodyReported(int actorNumber)
    {
        return _reportedDeadBodiesList.Contains(actorNumber);

    }
    public void ReportDeadBody()
    {
        if (DeadBodyInProximity == null) { return; }
        if (_reportedDeadBodiesList.Contains(DeadBodyInProximity.OwnerActorNr))
        {
            // the body was already reported do nothing
            return;
        }
        Debug.Log("DeadBodyInProximity.OwnerActorNr =" + DeadBodyInProximity.OwnerActorNr);
        photonView.RPC("ReportDeadBodyRPC", RpcTarget.All, DeadBodyInProximity.OwnerActorNr);

    }
    [PunRPC]
    public void ReportDeadBodyRPC(int actorNumber)
    {

        if (actorNumber != -1)
        {
            _reportedDeadBodiesList.Add(actorNumber);

        }
        // OnStartVoting();
        _playersThatHaveBeenVoteList.Clear();
        _playerThatVotedList.Clear();
        HasAlreadyVoted = false;
        ToggleAllButtons(true);

        PopulatePlayerList();
        _emergencyMeetingWindow.SetActive(true);
    }
    public void CastVote(int targetActorNumber)
    {
        if (LocalPlayer.hasMeetingAction)
        {
            LocalPlayer.MeetingAction(targetActorNumber);
        }
        else
        {
            ModeVote(targetActorNumber);
        }



    }
    [PunRPC]
    public void CastPlayerVoteRPC(int actorNumber, int targetActorNumber)
    {
        int remainingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - _reportedDeadBodiesList.Count - _playerThatHaveBeenKickedOutList.Count;
        // Set the status of the player that has just voted
        foreach (VotePlayerItem votePlayerItem in _votePlayerItemList)
        {
            if (votePlayerItem.ActorNumber == actorNumber)
            {
                votePlayerItem.UpdateStatus(targetActorNumber == -1 ? "SKIPPED" : "VOTED");
            }
        }
        // Log The player that just voted / and who voted for
        if (!_playerThatVotedList.Contains(actorNumber))
        {
            _playerThatVotedList.Add(actorNumber);
            _playersThatHaveBeenVoteList.Add(targetActorNumber);
        }


    }
    public IEnumerator ConcludeVote()
    {
        DestroyAllDeadBody();
        int remainingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - _reportedDeadBodiesList.Count - _playerThatHaveBeenKickedOutList.Count;

        // Count all the votes get actornumber from _playersThatHaveBeenVoteList
        Dictionary<int, int> playerVoteCount = new Dictionary<int, int>();
        foreach (int votedPlayer in _playersThatHaveBeenVoteList)
        {
            if (!playerVoteCount.ContainsKey(votedPlayer))
            {
                playerVoteCount.Add(votedPlayer, 0);
            }
            playerVoteCount[votedPlayer]++;


        }
        // Get the most voted player
        int mostVotedPlayer = -1;
        int mostVotes = int.MinValue;
        foreach (KeyValuePair<int, int> playerVote in playerVoteCount)
        {
            if (playerVote.Value > mostVotes)
            {
                mostVotes = playerVote.Value;
                mostVotedPlayer = playerVote.Key;
            }
            // Set CountVoteText for each player
            if (playerVote.Key != -1)
            {
                _votePlyaerItemList.Find((x) => x.ActorNumber == playerVote.Key).SetCountVoteText(playerVote.Value);

            }

        }

        yield return new WaitForSeconds(2.0f);
        // End the Voting session
        if ((mostVotes >= remainingPlayers / 2) && PhotonNetwork.IsMasterClient)
        {
            // kick the player or skip
            photonView.RPC("KickPlayerRPC", RpcTarget.All, mostVotedPlayer);
        }
        else
        {
            onEndVote.Invoke();
        }



    }
    [PunRPC]
    public void KickPlayerRPC(int actorNumber)
    {

        _emergencyMeetingWindow.SetActive(false);
        _kickPlayerWindow.SetActive(true);
        string playerName = string.Empty;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == actorNumber)
            {
                playerName = player.Value.NickName;
                break;
            }
        }
        _kickPlayerText.text = actorNumber == -1 ? "No one has been kicked out" : "Player " + playerName + " has been kicked out";
        StartCoroutine(FadeKickPlayerWindow(actorNumber));
    }
    private IEnumerator FadeKickPlayerWindow(int actorNumber)
    {
        yield return new WaitForSeconds(2.5f);
        _kickPlayerWindow.SetActive(false);
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            // spawn ghost in network.cs
            _network.DestroyPlayer();
            // _kickedPlayerWindow.SetActive(true);
            // yield return new WaitForSeconds(2.5f);
            // _kickedPlayerWindow.SetActive(false);
        }
        onEndVote.Invoke();



    }


    private void ToggleAllButtons(bool areOn)
    {
        _skipVoteBtn.interactable = areOn;
        foreach (VotePlayerItem votePlayerItem in _votePlyaerItemList)
        {
            votePlayerItem.ToggleButton(areOn);

        }
    }
    private void PopulatePlayerList()
    {
        // Clear the previous vote player list.
        for (int i = 0; i < _votePlyaerItemList.Count; i++)
        {
            Destroy(_votePlyaerItemList[i].gameObject);

        }
        _votePlyaerItemList.Clear();
        // Create new vote player list.
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // Do not add the current player to the list
            // if (player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            // {
            //     continue;
            // }

            // Do not Add the killed player to the list
            if (_reportedDeadBodiesList.Contains(player.Value.ActorNumber))
            {
                continue;
            }
            // Do not add the players that have been kicked out
            if (_playerThatHaveBeenKickedOutList.Contains(player.Value.ActorNumber))
            {
                continue;
            }
            VotePlayerItem newPlayerItem = Instantiate(_votePlayerItemPrefab, _votePlayerItemContainer);
            newPlayerItem.Initialize(player.Value, this);
            _votePlyaerItemList.Add(newPlayerItem);
        }
        populateRoleList();
    }

    private void populateRoleList()
    {
        foreach (KeyValuePair<MasterClient.Role, string> _role in allroledict)
        {
            GameObject _btn = Instantiate(RoleBtn, ChooseRoleContainer.transform);
            _btn.GetComponent<Button>().onClick.AddListener(() => ChooseRoleBtn(_role.Key));
            _btn.GetComponentInChildren<Text>().text = _role.Value;
        }

        // for (int i = 0; i < RoleBtnGroup.Count; i++)
        // {
        //     RoleBtnGroup[i].onClick.AddListener(() => ChooseRoleBtn(i));
        //     RoleBtnGroup[i].GetComponentInChildren<Text>().text = i.ToString();


        // }
    }
    private void DestroyAllDeadBody()
    {
        PlayerDeadBody[] _playerDeadBodys = FindObjectsOfType<PlayerDeadBody>();
        foreach (PlayerDeadBody deadbody in _playerDeadBodys)
        {
            Destroy(deadbody.gameObject);

        }
    }

    public void ModeVote(int targetActorNumber)
    {
        // Do not Add the killed player to vote
        if (_reportedDeadBodiesList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            return;
        }
        // Do not add the players that have been kicked out to vote
        if (_playerThatHaveBeenKickedOutList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            return;
        }
        if (HasAlreadyVoted) { return; }
        HasAlreadyVoted = true;
        ToggleAllButtons(false);
        photonView.RPC("CastPlayerVoteRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, targetActorNumber);
    }
    public void KillInMeeting(int currentTargetActorNumber)
    {
        photonView.RPC("KillInMeetingRPC", RpcTarget.All, currentTargetActorNumber);

    }
    [PunRPC]
    public void KillInMeetingRPC(int _targetActorNumber)
    {
        // find playerlist gameobject from actornumber
        VotePlayerItem playerlistobj = _votePlyaerItemList.Find(x => x.ActorNumber == _targetActorNumber);
        playerlistobj.ShowDead();
        playerlistobj.ToggleButton(false);
        // find player gameobject from actornumber
        if (PhotonNetwork.LocalPlayer.ActorNumber == _targetActorNumber)
        {
            GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Network>().DestroyPlayer();
            _reportedDeadBodiesList.Add(_targetActorNumber);
        }
    }
    public MasterClient.Role CheckRoleOfPlayer(int _targetActorNumber)
    {
        List<Playerinfo> allplayerinfo = new List<Playerinfo>(FindObjectsOfType<Playerinfo>());
        return allplayerinfo.Find(x => x.ActorNumber == _targetActorNumber).GetComponent<Role>().role;
    }

    public void ChooseRoleBtn(MasterClient.Role _role)
    {
        CurrentChooseRole = _role;
        onChooseRole.Invoke();
    }
    public void DisablePlayerUIObj(int _targetActorNumber)
    {
        VotePlayerItem playerlistobj = _votePlyaerItemList.Find(x => x.ActorNumber == _targetActorNumber);
        playerlistobj.ToggleButton(false);

    }
    public bool CheckIfPlayerIsImpostor(int _targetActorNumber)
    {
        List<Playerinfo> allplayerinfo = new List<Playerinfo>(FindObjectsOfType<Playerinfo>());
        Debug.Log("This player role = " + allplayerinfo.Find(x => x.ActorNumber == _targetActorNumber).GetComponent<Role>().role);
        return MasterClient.VirusRoleList.Contains(allplayerinfo.Find(x => x.ActorNumber == _targetActorNumber).GetComponent<Role>().role);
    }





}
