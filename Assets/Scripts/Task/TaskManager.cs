using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
// at start of the game assign Task to The Player 
// Sync Task progression On Network
// Show personal task list
// Update Task list and task progression when task complete
public class TaskManager : MonoBehaviourPun
{
    [SerializeField] private List<Interactible> AllTaskInteraction;
    private List<Interactible> CurrentTask;
    [SerializeField] private Text TaskDescriptionPrefab;
    [SerializeField] private GameObject TaskListContainer;
    private List<int> AllTaskInd;
    private int currentProgress;
    public int TaskCount = 5;
    private void Awake()
    {
        DisableAlltask();
        RandomTask();
    }

    private void RandomTask()
    {
        AllTaskInd.Clear();
        for (int i = 0; i < AllTaskInteraction.Count; i++) { AllTaskInd.Add(i); }
        for (int i = 0; i < TaskCount; i++)
        {
            int _randomInd = Random.Range(0, AllTaskInteraction.Count);
            AllTaskInteraction[_randomInd].gameObject.SetActive(true);
            Text newTaskDescription = Instantiate(TaskDescriptionPrefab, TaskListContainer.transform);
            newTaskDescription.text = AllTaskInteraction[_randomInd].taskDescription;

            CurrentTask.Add(AllTaskInteraction[_randomInd]);
            AllTaskInd.Remove(_randomInd);
        }


    }
    private void DisableAlltask()
    {
        foreach (Interactible _interactible in AllTaskInteraction)
        {
            TaskDescriptionPrefab.gameObject.SetActive(false);
        }


    }

}
