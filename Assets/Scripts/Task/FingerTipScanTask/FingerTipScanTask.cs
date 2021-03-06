using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FingerTipScanTask : Task
{
    [SerializeField] private Slider slider;
    private void OnEnable()
    {
        if (slider != null)
        {
            slider.value = slider.minValue;
        }
    }
    private void Update()
    {
        if (slider.value == slider.maxValue)
        {
            base.OnComplete();
        }
    }




}
