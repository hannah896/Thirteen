using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipType 
{
    Weapon,
    Tool,
    Armor
}

public class Equipment : MonoBehaviour
{
    ItemData data;
    public EquipType type;

    public float workspeed;
    public float hitDistance;

    [Header("Weapon")]
    public float AttackPower;


}
