using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour, IInteractable
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