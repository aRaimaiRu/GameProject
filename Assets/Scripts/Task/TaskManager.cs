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
    private int AntiVirusLeftRoomCount;
    public static TaskManager Instance;



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        // Debug.Log("Virus number =" + (int)PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"]);
        // Debug.Log("all number =" + PhotonNetwork.CurrentRoom.PlayerCount);

        DisableAlltask();
        List<Playerinfo> allplayerinfo = new List<Playerinfo>(FindObjectsOfType<Playerinfo>());
        if (!VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber))
        {
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
    }

    private void CountTask()
    {

        AllTaskCount = (_masterClient.AntiVirusCount - AntiVirusLeftRoomCount) * TaskCount;
        Debug.Log("AllTaskCount =" + AllTaskCount);
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        // check if player that left is impostor?
        if (VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            AntiVirusLeftRoomCount++;
            CountTask();
        }
    }




}
