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
        bool isCraft = true;

        //인벤토리에서 재료 차감 후 생산 아이템 추가
        if (item.cost.Count > 0)
        {
            isCraft = InventoryManager.Instance.consumeInventory.ConsumeResources(item.cost);
        }

        if (isCraft)
        {
            CharacterManager.Instance.Player.itemData = item;

            CharacterManager.Instance.Player.AddItem();
        }
    }
}
