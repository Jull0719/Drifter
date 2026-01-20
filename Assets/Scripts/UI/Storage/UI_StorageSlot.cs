using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        storage.FromStorageToPlayer(itemInSlot);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}
