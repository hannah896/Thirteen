using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventory : UIInventory
{

    public GameObject equipButton;
    public GameObject unequipButton;

    protected override void ClearSelectedItemWindow()
    {
        base.ClearSelectedItemWindow();

        equipButton.SetActive(false);
        unequipButton.SetActive(false);
    }

    public override void SelectItem(int index)
    {
        base.SelectItem(index);

        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipped);
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

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

    public void OnEquipButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    public void OnUnEquipButton()
    {

    }
}
