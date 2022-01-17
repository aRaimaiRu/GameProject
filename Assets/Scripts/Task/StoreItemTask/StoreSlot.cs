using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour, IDropHandler
{
    public StoreItemTask.ItemType acceptType;
    public bool isfullfill = false;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Slot on drop");
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<StoreItem>().thisItemType == acceptType)
        {
            // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            RectTransform elementRect = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform targetRect = GetComponent<RectTransform>();

            var toDestinationInWorldSpace = targetRect.position - elementRect.position;
            var toDestinationInLocalSpace = elementRect.InverseTransformVector(toDestinationInWorldSpace);
            elementRect.anchoredPosition = elementRect.anchoredPosition + (Vector2)toDestinationInLocalSpace;
            eventData.pointerDrag.GetComponent<StoreItem>().SetDragable(false);
            isfullfill = true;
        }

    }
}
