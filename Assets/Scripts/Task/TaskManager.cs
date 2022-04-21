using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using RoleList;
using UnityEngine;
using UnityEngine.UI;
// at start of the game assign Task to The Player 
// Sync Task progression On Network
// Show personal task list
// Update Task list and task progression when task complete

// masterclient send RPC to Initialize all task to each player?
// First create task and send RPC to all player create task += 1
//  second if player leave is create Task -= 1
public class TaskManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Slider _slider;
    [SerializeField] private List<Interactible> AllTaskInteraction;
    private Dictionary<Interactible, GameObject> testCurrentTaskList = new Dictionary<Interactible, GameObject>();
    [SerializeField] private GameObject TaskDescriptionPrefab;
    [SerializeField] private GameObject TaskListContainer;
    private List<int> AllTaskInd = new List<int>();
    private List<int> AllCurrentTaskInd = new List<int>();
    public int TaskCount = 5;
    private int AllTaskCount = 5;
    public static TaskManager Instance;
    private Dictionary<int, int> ActorNumberAndTaskCount = new Dictionary<int, int>();
    public Dictionary<int, bool> ActorNumberAndIsDead = new Dictionary<int, bool>();
    private string CustomPropKey = "ActorNumberTaskKey";
    private int globalAllTaskCount;
    public int ImpostorCount;
    public int AntiVirusCount;
    public List<int> markedActorNumber = new List<int>();
    public event Action onAntiVirusWin;
    public event Action onVirusWin;
    public event Action<int> OnPlayerLeftRoomEvent;

    public event Action<int> OnPlayerKilledEvent;
    private bool isEnd = false;

    #region SabotageProperties

    public event Action onLightSabotage;
    public event Action<int> onDoorSabotage;
    public GameObject SabotageMenu;
    public void onDoorSabotageTrigger(int id)
    {
        if (onDoorSabotage != null)
        {
            CloseSabotageMenu();
            UIControl.Instance.SabotageBtn.GetComponent<AbilityCooldownBtn>().RestartTimer();
            onDoorSabotage(id);
        }
    }
    public void onLightSabotageTrigger()
    {
        if (onLightSabotage != null)
        {
            CloseSabotageMenu();
            UIControl.Instance.SabotageBtn.GetComponent<AbilityCooldownBtn>().RestartTimer();
            onLightSabotage();
        }
    }



    #endregion
    public void OnPlayerKilledTrigger(int _actorNumber)
    {
        if (OnPlayerKilledEvent != null)
        {
            ActorNumberAndIsDead[_actorNumber] = false;
            OnPlayerKilledEvent(_actorNumber);
        }
    }





    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        VotingManager.Instance.onEndVote.AddListener(() =>
        {
            markedActorNumber.Clear();
            CheckEnd();
        });
        OnPlayerKilledEvent += CheckMarkedPlayerLeft;
        OnPlayerLeftRoomEvent += CheckMarkedPlayerLeft;
        // OnPlayerKilledEvent += (int i) => { CheckEnd(); };

    }

    public void Initialize()
    {
        StartCoroutine(DelayInitialize());

    }
    IEnumerator DelayInitialize()
    {
        yield return new WaitForSeconds(0.2f);
        VotingManager.Instance.ShowUISymbol();
        // Debug.Log("Virus number =" + (int)PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"]);
        // Debug.Log("all number =" + PhotonNetwork.CurrentRoom.PlayerCount);
        ImpostorCount = PhotonNetwork.CurrentRoom.PlayerCount < 3 ? 1 : (int)PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"];
        AntiVirusCount = PhotonNetwork.CurrentRoom.PlayerCount - ImpostorCount;
        DisableAlltask();
        List<Playerinfo> allplayerinfo = new List<Playerinfo>(FindObjectsOfType<Playerinfo>());
        if (!VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            Debug.Log("This player Not Impostor");
            RandomTask();
            popluateTaskUI();

        }
        Debug.Log("Delay init task TaskCount =" + TaskCount + " antivirus count =" + AntiVirusCount + " antivirus Count = " + AntiVirusCount);
        globalAllTaskCount = TaskCount * (AntiVirusCount);

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("IsDead", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        // CountTask();
    }

    private void RandomTask()
    {
        AllTaskInd.Clear();
        AllCurrentTaskInd.Clear();
        for (int i = 0; i < AllTaskInteraction.Count; i++) { AllTaskInd.Add(i); }
        for (int i = 0; i < TaskCount; i++)
        {
            int _randomInd = UnityEngine.Random.Range(0, AllTaskInd.Count);
            AllCurrentTaskInd.Add(AllTaskInd[_randomInd]);
            AllTaskInd.Remove(AllTaskInd[_randomInd]);

        }


    }
    private void popluateTaskUI()
    {
        foreach (int _randomInd in AllCurrentTaskInd)
        {
            // set Interactible active
            Interactible PickedInteractible = AllTaskInteraction[_randomInd];
            PickedInteractible.gameObject.SetActive(true);

            // instatiate TaskDescription
            GameObject newTaskDescription = Instantiate(TaskDescriptionPrefab, TaskListContainer.transform);
            newTaskDescription.GetComponentInChildren<Text>().text = PickedInteractible.taskDescription;
            newTaskDescription.SetActive(true);

            testCurrentTaskList.Add(PickedInteractible, newTaskDescription);
        }
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(CustomPropKey, AllCurrentTaskInd.Count);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }
    private void DisableAlltask()
    {
        foreach (Interactible _interactible in AllTaskInteraction)
        {
            TaskDescriptionPrefab.gameObject.SetActive(false);
        }


    }
    public void CompleteTask(Interactible _interactible)
    {
        Destroy(testCurrentTaskList[_interactible].gameObject);
        testCurrentTaskList.Remove(_interactible);
        _interactible.gameObject.SetActive(false);

        Debug.Log("test Current Task list count = " + testCurrentTaskList.Count + "  " + PhotonNetwork.LocalPlayer.ActorNumber);

        photonView.RPC("SetActorNumberAndTaskCount", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, testCurrentTaskList.Count);
    }

    private void CountTask()
    {
        int buffer = 0;
        foreach (KeyValuePair<int, int> actor in ActorNumberAndTaskCount)
        {
            buffer += actor.Value;
        }
        // AllTaskCount = (_masterClient.AntiVirusCount - AntiVirusLeftRoomCount) * TaskCount;
        AllTaskCount = buffer;
        Debug.Log("AllTaskCount =" + AllTaskCount);
        CountProgress();
        CheckEndByTask();

    }
    public void OnPlayerLeftRoomCustom(Player newPlayer)
    {
        if (OnPlayerLeftRoomEvent != null) { OnPlayerLeftRoomEvent(newPlayer.ActorNumber); }
        UpdateCurrentPlayerCount(newPlayer);
        SetActorNumberAndTaskCount(newPlayer.ActorNumber, 0);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("IsDead"))
        {
            Debug.Log("IsDead props");
            ActorNumberAndIsDead[targetPlayer.ActorNumber] = (bool)changedProps["IsDead"];

        }
        if (!changedProps.ContainsKey(CustomPropKey))
        {
            return;
        }
        Debug.Log(targetPlayer.ActorNumber + "" + changedProps[CustomPropKey]);
        SetActorNumberAndTaskCount(targetPlayer.ActorNumber, (int)changedProps[CustomPropKey]);

    }
    [PunRPC]
    public void SetActorNumberAndTaskCount(int _actorNumber, int _Count)
    {
        Debug.Log("isPlayer = Impostor ?" + VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber));

        ActorNumberAndTaskCount[_actorNumber] = _Count;
        CountTask();
    }
    private void UpdateCurrentPlayerCount(Player _LeftedPlayer)
    {
        if (VotingManager.Instance.CheckIfPlayerIsImpostor(_LeftedPlayer.ActorNumber))
        {
            ImpostorCount -= 1;

        }
        else
        {
            AntiVirusCount -= 1;
        }
    }


    public void CountProgress()
    {

        Debug.Log("Count progress = " + globalAllTaskCount + " " + AllTaskCount);
        _slider.maxValue = globalAllTaskCount;
        _slider.minValue = 0;
        _slider.value = globalAllTaskCount - AllTaskCount;

    }

    #region Sabotage
    public void OpenSabotageMenu()
    {
        SabotageMenu.SetActive(true);
    }
    public void CloseSabotageMenu()
    {
        SabotageMenu.SetActive(false);
    }

    #endregion

    #region CheckEndGame
    public void CheckEndByTask()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;
        Debug.Log("CheckEndByTask all Task Count" + AllTaskCount);
        if (AllTaskCount == 0)
        {
            AnitiVirusWin();
        }
    }
    [PunRPC]
    public void CheckEndMasterClient()
    {
        Debug.Log("CheckEndMaster maskcount =" + markedActorNumber.Count + " AntiVirusCount =" + PlayerManager.Instance.AntiVirusCount);
        if (isEnd) return;
        if (AllTaskCount == 0)
        {
            Debug.Log("End By TASKKKKK");
            AnitiVirusWin();
        }
        else if (markedActorNumber.Count >= PlayerManager.Instance.AntiVirusCount)
        {
            // Debug.Log("End By Worm" + markedActorNumber.Count + Player.Instance.AllRoleList.FindAll(x => RoleListClass.AntiVirusRoleList.Contains(x.role)).Count);
            VirusWin();
        }
        else if (PlayerManager.Instance.AntiVirusCount <= PlayerManager.Instance.VirusCount)
        {
            Debug.Log("End By Count");
            VirusWin();
        }
        else if (PlayerManager.Instance.VirusCount <= 0)
        {
            Debug.Log("No Virus" + PlayerManager.Instance.VirusCount);
            AnitiVirusWin();
        }
    }
    public void CheckEnd()
    {
        photonView.RPC("CheckEndMasterClient", RpcTarget.MasterClient);
    }
    // Show Victory to AnitiVirus
    // Show Defeat to Virus
    // press to Back to lobby
    public void AnitiVirusWin()
    {
        Debug.Log("all Task Count" + AllTaskCount);
        isEnd = true;
        photonView.RPC("AnitiVirusWinRPC", RpcTarget.All);
    }
    public void VirusWin()
    {
        Debug.Log("Virus Win all Task Count" + AllTaskCount);
        isEnd = true;
        photonView.RPC("VirusWinRPC", RpcTarget.All);


    }
    [PunRPC]
    public void AnitiVirusWinRPC()
    {

        onAntiVirusWin();

    }
    [PunRPC]
    public void VirusWinRPC()
    {

        onVirusWin();

    }

    public void BackToLobby()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LoadLevel("Lobby1");
    }
    #endregion

    #region Worm
    public void CheckMarkedPlayerLeft(int _ActorNumber)
    {
        if (markedActorNumber.Contains(_ActorNumber))
        {
            markedActorNumber.Remove(_ActorNumber);
        }
        // CheckeEndByWorm();

    }

    public void Addmarked(int _ActorNumber)
    {
        photonView.RPC("AddmarkedRPC", RpcTarget.All, _ActorNumber);
    }
    [PunRPC]

    public void AddmarkedRPC(int _ActorNumber)
    {
        Debug.Log("Addd number = " + _ActorNumber);
        markedActorNumber.Add(_ActorNumber);
    }
    #endregion





}
