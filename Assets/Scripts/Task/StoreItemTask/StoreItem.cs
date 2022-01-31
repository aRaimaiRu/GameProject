using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StoreItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    private bool dragable;
    public StoreArrEnum.ItemType thisItemType;
    private RectTransform rectTransform;


    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponentInParent<CanvasGroup>();
        // _canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        dragable = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (!dragable) { return; }
        rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        if (!dragable) { return; }

        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");

        _canvasGroup.blocksRaycasts = true;

    }
    public void SetDragable(bool _setDraggable)
    {
        dragable = _setDraggable;
    }
}
