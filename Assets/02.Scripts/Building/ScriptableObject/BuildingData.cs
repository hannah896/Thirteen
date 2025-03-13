using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "New Building")]

public class BuildingData : ScriptableObject
{
    [Header("Info")]
    public string displayName;      //�̸�
    public string description;      //����
    public Sprite icon;             //������
    public GameObject buildPrefab;  //�ǹ�

    [Header("Construction Requirements")]
    public List<ResourceCost> requiredResources;    //�ڿ� ����
}

[Serializable]
public class ResourceCost
{
    public ItemData resource;   //��� ������
    public int amount;          //�ʿ� ����
}