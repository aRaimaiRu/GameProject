using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class VotingManager : MonoBehaviourPun
{
    public static VotingManager Instance;
    [HideInInspector] public PhotonView DeadBodyInProximity;
    private List<int> _reportedDeadBodiesList = new List<int>();
    [SerializeField] private GameObject _emergencyMeetingWindow;
    [SerializeField] private VotePlayerItem _votePlayerItemPrefab;
    [SerializeField] private Transform _votePlayerItemContainer;
    private List<VotePlayerItem> _votePlyaerItemList = new List<VotePlayerItem>();
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


    private void Awake()
    {
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

        _reportedDeadBodiesList.Add(actorNumber);
        _playersThatHaveBeenVoteList.Clear();
        _playerThatVotedList.Clear();
        ToggleAllButtons(true);

        PopulatePlayerList();
        _emergencyMeetingWindow.SetActive(true);
    }
    public void CastVote(int targetActorNumber)
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
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (_playerThatVotedList.Count < remainingPlayers)
        {
            return;

        }
        // ConcludeVote()

    }
    public void ConcludeVote()
    {
        int remainingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - _reportedDeadBodiesList.Count - _playerThatHaveBeenKickedOutList.Count;

        // Count all the votes
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
            _votePlyaerItemList.Find((x) => x.ActorNumber == playerVote.Key).SetCountVoteText(playerVote.Value);

        }

        if (!PhotonNetwork.IsMasterClient) { return; }
        // End the Voting session
        if (mostVotes >= remainingPlayers / 2)
        {
            // kick the player or skip
            photonView.RPC("KickPlayerRPC", RpcTarget.All, mostVotedPlayer);

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
        _kickPlayerText.text = actorNumber == -1 ? "No one has been kicked out" : "Player " + playerName + "has been kicked out";
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
            _kickedPlayerWindow.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            _kickedPlayerWindow.SetActive(false);
        }


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
    }


}
