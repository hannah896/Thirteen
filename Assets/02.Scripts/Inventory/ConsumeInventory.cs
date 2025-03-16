using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeInventory : UIInventory
{
    // consume
    public GameObject useButton;

    protected override void ClearSelectedItemWindow()
    {
        base.ClearSelectedItemWindow();

        useButton.SetActive(false);
    }

    public override void SelectItem(int index)
    {
        base.SelectItem(index);

        useButton.SetActive(selectedItem.itemType == ItemType.Consumable);
    }
    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            slot.quantity++;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
        }

        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    /* public void OnUseButton()
     {
         if(selectedItem.itemType == ItemType.Consumable)
         {
             for(int i = 0; i < 
         }
     }*/
}
