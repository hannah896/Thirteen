using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public List<ItemData> itemList;

    public bool CanCraft(ItemData item)
    {
        //제작에 필요한 아이템 확인
        return InventoryManager.Instance.consumeInventory.HasRequiredResources(item.cost);
    }

    public void CraftItem(ItemData item)
    {
        //인벤토리에서 재료 차감 후 생산 아이템 추가
        if (InventoryManager.Instance.consumeInventory.ConsumeResources(item.cost))
        {
            CharacterManager.Instance.Player.itemData = item;

            CharacterManager.Instance.Player.AddItem();
        }
    }
}
