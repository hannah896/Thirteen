using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData data;

    public float workspeed;
    public float hitDistance;

    [Header("Weapon")]
    public float AttackPower;

    [Header("Armor")]
    public float DefeatPower;
}