using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject useButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

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

        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = 1;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
        Debug.Log("인벤토리");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if(IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
        
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
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
        if(emptySlot != null)
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
    void UpdateUI()
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
    ItemSlot GetItemStack(ItemData data)
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

    ItemSlot GetEmptySlot()
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

    void ThrowItem(ItemData data)
    {
        Instantiate(data.equipPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
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

        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipped);
        useButton.SetActive(selectedItem.itemType == ItemType.Consumable);
        dropButton.SetActive(true);
    }

   /* public void OnUseButton()
    {
        if(selectedItem.itemType == ItemType.Consumable)
        {
            for(int i = 0; i < 
        }
    }*/

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity --;

        if(slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
}
