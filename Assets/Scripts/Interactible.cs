using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] private GameObject _taskWindow;
    [SerializeField] private Sprite _InteractibleHighlight;
    [SerializeField] private Sprite SourceSprite;
    public string taskDescription;
    private UIControl _uiControl;
    private void Start()
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
    private void FixedUpdate()
    {
        if (_InteractibleHighlight == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = (UIControl.Instance.CurrentInteractible == this) ? _InteractibleHighlight : SourceSprite;
    }
    public void Use(bool isActive)
    {
        _taskWindow.SetActive(isActive);

    }





}
