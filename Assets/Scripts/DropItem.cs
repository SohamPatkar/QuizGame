using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject Dropped = eventData.pointerDrag;
        DraggableItem draggableItem = Dropped.GetComponent<DraggableItem>();
        draggableItem.parentSlot = transform;
    }
}
