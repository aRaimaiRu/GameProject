using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public Interactible thisInteractible;
    public void OnComplete()
    {
        this.gameObject.SetActive(false);
        TaskManager.Instance.CompleteTask(thisInteractible);
    }
}
