using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FingerTipScanBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image bg;
    [SerializeField] private Color pressedColor;
    [SerializeField] private Color normalColor;

    private bool hold;

    public void OnPointerDown(PointerEventData eventData)
    {
        hold = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hold = false;


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        hold = false;
    }
    private void Update()
    {
        if (hold)
        {
            slider.value += Time.deltaTime;
            bg.color = pressedColor;
        }
        else
        {
            slider.value -= Time.deltaTime;
            bg.color = normalColor;
        }
    }

}
