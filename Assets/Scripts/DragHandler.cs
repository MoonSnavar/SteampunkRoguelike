using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{    
    public static int currentIndexItem;
    public static GameObject ItemBeingDragged;
    public bool canDrag = false;
    public static Transform StartParent;
    private Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        
        ItemBeingDragged = gameObject;
        startPosition = transform.position;
        StartParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        for (int i = 0; i < transform.parent.transform.parent.childCount; i++)
        {
            if (transform.parent.name == transform.parent.transform.parent.GetChild(i).name)
            {
                currentIndexItem = i;
                break;
            }
        }

        transform.SetParent(GameManager.instance.Canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        ItemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == StartParent || transform.parent == GameManager.instance.Canvas.transform)
        {
            transform.position = startPosition;
            transform.SetParent(StartParent);
        }
    }
}
