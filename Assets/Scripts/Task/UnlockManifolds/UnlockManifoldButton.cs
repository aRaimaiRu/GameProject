using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManifoldButton : MonoBehaviour
{
    private int _value;
    private Text _buttonText;
    private Button _button;
    private UnlockManifoldsTask _parentTask;
    public void Initialize(int value, UnlockManifoldsTask parentTask)
    {
        if (_buttonText == null)
        {
            _buttonText = GetComponentInChildren<Text>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);

        }
        _buttonText.text = value.ToString();
        _parentTask = parentTask;
        _value = value;



    }
    public void ToggleButton(bool isOn)
    {
        _button.interactable = isOn;

    }
    private void OnButtonClicked()
    {
        _parentTask.OnButtonPressed(_value, this);
    }
}
