using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData data;

    // 장비의 local 정보
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    public float workspeed;
    public float hitDistance;

    [Header("Weapon")]
    public float AttackPower;
}