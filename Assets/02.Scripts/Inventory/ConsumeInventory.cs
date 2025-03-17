using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeInventory : UIInventory
{
    //consume
    public GameObject useButton;

    protected override void ClearSelectedItemWindow()
    {
        base.ClearSelectedItemWindow();

        useButton.SetActive(false);
    }

    public override void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        base.SelectItem(index);

        useButton.SetActive(selectedItem.itemType == ItemType.Consumable);
    }
    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
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

    public void OnUseButton()
    {
        if (selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.effect.Length; i++)
            {
                switch (selectedItem.effect[i].consumableType)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.effect[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem);
                        break;
                    case ConsumableType.Thirsty:
                        condition.Eat(selectedItem);
                        break;
                    case ConsumableType.Stemina:
                        condition.Eat(selectedItem);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }
}
