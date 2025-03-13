using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "New Building")]

public class BuildingData : ScriptableObject
{
    [Header("Info")]
    public string displayName;      //이름
    public string description;      //설명
    public Sprite icon;             //아이콘
    public GameObject buildPrefab;  //건물

    [Header("Construction Requirements")]
    public List<ResourceCost> requiredResources;    //자원 정보
}

[Serializable]
public class ResourceCost
{
    public ItemData resource;   //재료 아이템
    public int amount;          //필요 수량
}