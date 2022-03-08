using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public Interactible thisInteractible;
    public void OnComplete()
    {
        thisInteractible = UIControl.Instance.CurrentInteractible;
        this.gameObject.SetActive(false);
        thisInteractible.gameObject.SetActive(false);

        TaskManager.Instance.CompleteTask(thisInteractible);
    }
}
