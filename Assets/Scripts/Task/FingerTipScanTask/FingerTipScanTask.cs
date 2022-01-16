using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FingerTipScanTask : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Update()
    {
        if (slider.value == slider.maxValue)
        {
            this.gameObject.SetActive(false);
        }
    }




}
