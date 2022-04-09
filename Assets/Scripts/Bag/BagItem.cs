using UnityEngine;
using UnityEngine.EventSystems;
using CustomDrag;

public class BagItem : DraggableImage, IPointerDownHandler, IPointerUpHandler
{
    public ItemBox itemBox;
    private bool isHold;
    private Vector2 pointerDownPosition;

    private void Update()
    {
        if (isHold && !IsDragging)
        {
            if (Vector2.Distance(BagManager.Instance.GetMousePosition(), pointerDownPosition) > 30)
            {
                base.OnPointerDown(null);
                BagManager.Instance.UnfocusItem();
                BagManager.Instance.FocusItem(itemBox.Index);
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        BagManager.Instance.OnItemPointerDown(enable, out pointerDownPosition);
        if (enable) isHold = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        BagManager.Instance.OnItemPointerUp(IsDragging);
        base.OnPointerUp(eventData);
        isHold = false;
    }
}