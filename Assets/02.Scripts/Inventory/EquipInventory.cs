using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventory : UIInventory
{

    public GameObject equipButton;      //장착 버튼
    public GameObject unequipButton;    //해제 버튼

    public int equipIndex;              // 장착 중인 장비 슬롯 인덱스

    protected override void ClearSelectedItemWindow()
    {
        base.ClearSelectedItemWindow(); //base = 클래스의 상속받은 부모클래스를 뜻한다. 
                                        //base.ClearSelectedItemWindow(); : base 클래스 즉, UI Inventory 정의된 ClearselectedItemWindow라는 메서드를 실행

        equipButton.SetActive(false);   //장착버튼 비활성화
        unequipButton.SetActive(false); //해제버튼 비활성화
    }

    public override void SelectItem(int index)
    {
        if (slots[index].item == null) return;
        base.SelectItem(index);

        UpdateEquipButton(index);

        UpdateDropButton(slots[selectedItemIndex].equipped);
    }

    public void UpdateEquipButton(int index)
    {
        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipped);
    }

    public void UpdateDropButton(bool isEquipped)
    {
        dropButton.SetActive(!isEquipped);
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
        // 선택한 아이템이 장비가 아니면 반환
        if (selectedItem == null || selectedItem.itemType != ItemType.Equipable) return;

        // 이전에 장착한 아이템이 있다면 해제 해주기
        if (equipIndex > -1)
        {
            slots[equipIndex].equipped = false;
        }
        slots[selectedItemIndex].equipped = true;
        // 장착 시 equipIndex 할당
        equipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equipment.Equip(selectedItem);
        UpdateEquipButton(selectedItemIndex);
        UpdateDropButton(slots[selectedItemIndex].equipped);
        UpdateUI();
    }

    public void OnUnEquipButton()
    {
        if (selectedItem == null || selectedItem.itemType != ItemType.Equipable) return;

        slots[equipIndex].equipped = false;
        UpdateEquipButton(equipIndex);
        // 해제 시 equipIndex -1 할당
        equipIndex = -1;
        CharacterManager.Instance.Player.equipment.UnEquip();
        UpdateUI();
    }
}
