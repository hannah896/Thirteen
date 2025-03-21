using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    Bat,        // 야구배트
    Axe,        // 도끼
    Hammer,     // 망치
    Machete,    // 마체테
    Shovel      // 삽
}

public class Equipment : MonoBehaviour, IInteractable
{
    public ItemData data;
    public EquipType equipType;

    // 장비의 local 정보
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    public float workspeed;
    public float hitDistance;

    [Header("Weapon")]
    public float AttackPower;

    private Interaction interaction;

    private void Start()
    {
        interaction = CharacterManager.Instance.Player.GetComponent<Interaction>();
    }
    public void OnInteraction()
    {
        CharacterManager.Instance.Player.interactionUI.SetActive(true);
        CharacterManager.Instance.Player.interactionUI.Set(data);
        interaction.SetItemData(data);
    }
}