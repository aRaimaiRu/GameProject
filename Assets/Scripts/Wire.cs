using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool IsLeftWire;
    public Color CustomColor;
    private Image _image;
    private LineRenderer _lineRenderer;
    private Canvas _canvas;
    private bool _isDragStarted = false;
    private WireTask _wireTask;
    public bool IsSuccess = false;
    public void Initialize()
    {
        _image = GetComponent<Image>();
        _lineRenderer = GetComponent<LineRenderer>();
        _canvas = GetComponentInParent<Canvas>();
        _wireTask = GetComponentInParent<WireTask>();
        _isDragStarted = false;
        IsSuccess = false;
        _lineRenderer.SetPosition(0, Vector2.zero);
        _lineRenderer.SetPosition(1, Vector2.zero);

    }
    // private void OnEnable()
    // {
    //     _isDragStarted = false;
    //     IsSuccess = false;
    // }
    public void SetColor(Color color)
    {
        _image.color = color;
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
        CustomColor = color;
    }

    private void Update()
    {
        if (_isDragStarted)
        {
            Debug.Log("Dragging");
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out movePos);
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _canvas.transform.TransformPoint(movePos));


        }
        else
        {
            // Hide the line if not drag
            // if (IsSuccess)
            // {
            //     _lineRenderer.SetPosition(0, Vector2.zero);
            //     _lineRenderer.SetPosition(1, Vector2.zero);

            // }
        }
        bool isHoverd = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, _canvas.worldCamera);
        if (isHoverd)
        {
            _wireTask.CurrentHoverWire = this;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsLeftWire) { return; }
        if (IsSuccess) { return; }

        _isDragStarted = true;
        _wireTask.CurrentDraggedWire = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_wireTask.CurrentHoverWire != null)
        {
            if (_wireTask.CurrentHoverWire.CustomColor == CustomColor && !_wireTask.CurrentHoverWire.IsLeftWire)
            {
                IsSuccess = true;
                _wireTask.CurrentHoverWire.IsSuccess = true;
            }
        }
        _isDragStarted = false;
        _wireTask.CurrentDraggedWire = null;
    }
}
