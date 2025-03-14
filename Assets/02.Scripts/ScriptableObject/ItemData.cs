using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using VInspector;
public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger,
    Thirsty,
    Stemina
}

[CreateAssetMenu (fileName = "ItemBase", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemDescription;
    public Sprite Icon;
    public ItemType itemType;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Effect")]
    public Dictionary<ConsumableType, float> Effects;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Consume")]
    public int healAmount;

    [Header("Cost")]
    public List<ResourceCost> cost;
}
