using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    public bool endDragged = false;
    public bool atStart = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if(rectTransform != null)
        {
            startPosition = rectTransform.anchoredPosition;
        }
    }

    public void Update()
    {
        if (rectTransform.anchoredPosition != startPosition)
        {
            atStart = false;
        }
        else
        {
            atStart = true;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        endDragged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        endDragged = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        endDragged = true;
    }

    public void returnToStart()
    {
        rectTransform.anchoredPosition = startPosition;
    }
   
}
