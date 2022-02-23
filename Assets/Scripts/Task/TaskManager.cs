using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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
    public event Action onAntiVirusWin;
    public event Action onVirusWin;
    public event Action<int> OnPlayerLeftRoomEvent;
    public event Action<int> OnPlayerKilledEvent;

    #region SabotageProperties

    public event Action onLightSabotage;
    public event Action<int> onDoorSabotage;
    public GameObject SabotageMenu;
    public void onDoorSabotageTrigger(int id)
    {
        if (onDoorSabotage != null)
        {
            CloseSabotageMenu();
            onDoorSabotage(id);
        }
    }
    public void onLightSabotageTrigger()
    {
        if (onLightSabotage != null)
        {
            CloseSabotageMenu();
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
    public void Initialize()
    {
        StartCoroutine(DelayInitialize());

    }
    IEnumerator DelayInitialize()
    {
        yield return new WaitForSeconds(0.2f);
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
        _interactible.gameObject.SetActive(false);
        Destroy(testCurrentTaskList[_interactible].gameObject);
        testCurrentTaskList.Remove(_interactible);
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
    public override void OnPlayerLeftRoom(Player newPlayer)
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
    // Show Victory to AnitiVirus
    // Show Defeat to Virus
    // press to Back to lobby
    public void AnitiVirusWin()
    {
        Debug.Log("all Task Count" + AllTaskCount);
        photonView.RPC("AnitiVirusWinRPC", RpcTarget.All);
    }
    public void VirusWin()
    {
        Debug.Log("Virus Win all Task Count" + AllTaskCount);
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
        PhotonNetwork.LoadLevel("Lobby1");
    }
    #endregion

    public void GetKilled(int _playerActrNr)
    {
        photonView.RPC("GetKilledRPC", RpcTarget.All, _playerActrNr);
    }
    [PunRPC]
    public void GetKilledRPC(int _playerActrNr)
    {
        TaskManager.Instance.OnPlayerKilledTrigger(_playerActrNr);
    }



}
