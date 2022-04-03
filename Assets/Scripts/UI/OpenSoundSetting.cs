using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSoundSetting : MonoBehaviour
{
    public Button thisButton;
    private void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            AudioManager.instance.setCanvasActive(true);
        });
    }


}
