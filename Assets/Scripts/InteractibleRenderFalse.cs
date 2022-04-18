using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractibleRenderFalse : Interactible
{
    public override void Use(bool isActive)
    {
        foreach (Image img in _taskWindow.GetComponentsInChildren<Image>())
        {
            img.enabled = isActive;
        }

    }
}
