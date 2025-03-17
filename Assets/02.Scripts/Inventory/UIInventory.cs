using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum InventoryType
{ 
    Equip,
    Item
}


public class UIInventory : MonoBehaviour
{
    public InventoryType inventoryType;

    public ItemSlot[] slots;

    protected ItemData selectedItem;
    protected int selectedItemIndex = 0;

    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject dropButton;

    protected PlayerController controller;
    protected PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        if (CharacterManager.Instance == null)
        {
            Debug.Log("CharacterManager.Instance가 null입니다. CharacterManager가 씬에 존재하는지 확인하세요.");
            return;
        }

        if (CharacterManager.Instance.Player == null)
        {
            Debug.Log("CharacterManager.Instance.Player가 null입니다. Player 객체가 정상적으로 초기화되었는지 확인하세요.");
            return;
        }

        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        if (controller == null)
        {
            Debug.Log("Player.controller가 null입니다. Player 스크립트에서 controller가 올바르게 할당되었는지 확인하세요.");
            return;
        }

        if (condition == null)
        {
            Debug.Log("Player.condition이 null입니다. Player 스크립트에서 condition이 올바르게 할당되었는지 확인하세요.");
            return;
        }

        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
        UpdateUI();
        Debug.Log("인벤토리");
    }

    protected virtual void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        dropButton.SetActive(false);
    }
    protected void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    // 동일한 아이템을 가지고 있는지 확인하고 반환
    protected ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i<slots.Length; i++)
        {
            if (slots[i].item == data  && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }
    // 비어있는 슬롯이 있는지 확인하고 반환
    protected ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 아이템 버리기
    protected void ThrowItem(ItemData data)
    {
        GameObject dropItem = Instantiate(data.equipPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        if(dropItem.TryGetComponent(out Collider collider))
        {
            collider.enabled = true;
        }
        else
        {
            dropItem.AddComponent<BoxCollider>();
        }

        if(!dropItem.TryGetComponent(out Rigidbody rigid))
        {
            dropItem.AddComponent<Rigidbody>();
        }
    }

    public virtual void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.itemDescription;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        foreach (var item in selectedItem.effect)
        {
            selectedStatName.text += item.consumableType.ToString();
            selectedStatValue.text += item.value.ToString();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    protected void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity --;

        if(slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public bool HasRequiredResources(List<ResourceCost> data)
    {
        bool found = false;

        //필요한 재료 인벤토리에 존재하는지 확인
        foreach (ResourceCost cost in data)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == cost.resource & slots[i].quantity >= cost.amount)
                {
                    found = true;
                    break;
                }
            }

            if (!found) return false;
        }

        return true;
    }

    public bool ConsumeResources(List<ResourceCost> data)
    {
        if (!HasRequiredResources(data)) return false;

        foreach (ResourceCost cost in data)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == cost.resource)
                {
                    slots[i].quantity -= cost.amount;
                    if (slots[i].quantity <= 0)
                    {
                        slots[i] = null;
                    }
                    break;
                }
            }
        }

        return true;
    }
}
