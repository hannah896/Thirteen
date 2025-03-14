using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public List<ItemData> itemList;

    public bool CanCraft(ItemData item)
    {
        //인벤토리에서 필요한 아이템이 있는지 확인하는 메서드

        return true;
    }

    public void CraftItem(ItemData item)
    {
        //인벤토리에서 재료 차감
        //인벤토리에 아이템 추가
    }
}
