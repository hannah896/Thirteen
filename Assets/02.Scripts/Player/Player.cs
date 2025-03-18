using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InteractionUI interactionUI;

    public PlayerController controller;
    public PlayerCondition condition;
    public AnimationController animController;
    public EquipmentPlayer equipment;

    public ItemData itemData;
    public Resource resource;

    public Transform dropPosition;

    public Action inventory;

    public bool isDie;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        animController = GetComponent<AnimationController>();
        equipment = GetComponent<EquipmentPlayer>();

        interactionUI = FindObjectOfType<InteractionUI>();
    }

    public void AddItem()
    {
        if(itemData.itemType == ItemType.Equipable)
        {
            InventoryManager.Instance.equipInventory.AddItem();
        }
        else
        {
            InventoryManager.Instance.consumeInventory.AddItem();
        }
    }
}
