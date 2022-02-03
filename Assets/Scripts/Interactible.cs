using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] private GameObject _taskWindow;
    public string taskDescription;
    private void Start()
    {
        if (_taskWindow.GetComponent<Task>() != null)
        {
            _taskWindow.GetComponent<Task>().thisInteractible = this;
        }
        else if (_taskWindow.GetComponentInChildren<Task>() != null)
        {
            _taskWindow.GetComponentInChildren<Task>().thisInteractible = this;

        }
    }
    public void Use(bool isActive)
    {
        _taskWindow.SetActive(isActive);

    }

}
