using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SwipeCard : MonoBehaviour, IDragHandler
{
    private Canvas _canvas;

    // private void OnDrag(PointerEventData eventData)
    // {
    //     Vector2 pos;
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as ReactTransform,
    //     eventData.position, _canvas.worldCamera, out pos);
    // }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
        eventData.position, _canvas.worldCamera, out pos);
        transform.position = _canvas.transform.TransformPoint(pos);
    }
    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }
}
