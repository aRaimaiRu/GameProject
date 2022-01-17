using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorArrangeItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    private bool dragable;
    private RectTransform rectTransform;
    public ColorArrangeItem NextItem;
    public bool isComplete;

    private void Awake()
    {
        // _canvas = GetComponentInParent<Canvas>();
        // _canvasGroup = GetComponentInParent<CanvasGroup>();
        _canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        dragable = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (!dragable) { return; }
        rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
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

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Slot on drop");
        if (eventData.pointerDrag != null && eventData.pointerDrag == NextItem.gameObject)
        {
            // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            RectTransform elementRect = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform targetRect = GetComponent<RectTransform>();

            var toDestinationInWorldSpace = targetRect.position - elementRect.position;
            var toDestinationInLocalSpace = elementRect.InverseTransformVector(toDestinationInWorldSpace);
            elementRect.anchoredPosition = elementRect.anchoredPosition + (Vector2)toDestinationInLocalSpace;
            this.gameObject.SetActive(false);
            NextItem.isComplete = true;
        }

    }
    public void Setcolor(Color color)
    {
        this.GetComponent<Image>().color = color;
    }
}
