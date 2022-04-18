using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public GameObject _taskWindow;
    public Sprite _InteractibleHighlight;
    public Sprite SourceSprite;
    public string taskDescription;
    public UIControl _uiControl;
    public void Start()
    {
        SourceSprite = this.GetComponent<SpriteRenderer>().sprite;
        _uiControl = FindObjectOfType<UIControl>();
        if (_taskWindow.GetComponent<Task>() != null)
        {
            _taskWindow.GetComponent<Task>().thisInteractible = this;
        }
        else if (_taskWindow.GetComponentInChildren<Task>() != null)
        {
            _taskWindow.GetComponentInChildren<Task>().thisInteractible = this;

        }
    }
    public void FixedUpdate()
    {
        if (_InteractibleHighlight == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = (UIControl.Instance.CurrentInteractible == this) ? _InteractibleHighlight : SourceSprite;
    }
    public virtual void Use(bool isActive)
    {
        _taskWindow.SetActive(isActive);

    }





}
