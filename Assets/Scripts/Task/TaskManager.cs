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
    [SerializeField] private MasterClient _masterClient;
    [SerializeField] private List<Interactible> AllTaskInteraction;
    private Dictionary<Interactible, GameObject> testCurrentTaskList = new Dictionary<Interactible, GameObject>();
    [SerializeField] private GameObject TaskDescriptionPrefab;
    [SerializeField] private GameObject TaskListContainer;
    private List<int> AllTaskInd = new List<int>();
    private List<int> AllCurrentTaskInd = new List<int>();
    private int currentProgress;
    public int TaskCount = 5;
    private int AllTaskCount = 0;
    public static TaskManager Instance;
    private Dictionary<int, int> ActorNumberAndTaskCount = new Dictionary<int, int>();
    private string CustomPropKey = "ActorNumberTaskKey";



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    public void Initialize()
    {
        // Debug.Log("Virus number =" + (int)PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"]);
        // Debug.Log("all number =" + PhotonNetwork.CurrentRoom.PlayerCount);

        DisableAlltask();
        List<Playerinfo> allplayerinfo = new List<Playerinfo>(FindObjectsOfType<Playerinfo>());
        if (!VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            Debug.Log("This player Not Impostor");
            RandomTask();
            popluateTaskUI();
        }
        CountTask();

    }

    private void RandomTask()
    {
        AllTaskInd.Clear();
        AllCurrentTaskInd.Clear();
        for (int i = 0; i < AllTaskInteraction.Count; i++) { AllTaskInd.Add(i); }
        for (int i = 0; i < TaskCount; i++)
        {
            int _randomInd = Random.Range(0, AllTaskInd.Count);
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
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        // check if player that left is impostor?
        // if (VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber))
        // {
        // AntiVirusLeftRoomCount++;
        SetActorNumberAndTaskCount(newPlayer.ActorNumber, 0);

        // }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
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
        ActorNumberAndTaskCount[_actorNumber] = _Count;
        Debug.Log("isPlayer = Impostor ?" + VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber));
        CountTask();
    }


    // [PunRPC]
    // public void InitiateTaskRPC(int actorNumber,)
    // {
    // }




}
