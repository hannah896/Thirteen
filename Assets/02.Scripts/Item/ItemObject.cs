using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteraction();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData ItemData;
    // TODO: 인벤토리 만들어졌을때 넣는 기능 필요.

    private InteractionUI interactionUI;

    private void Start()
    {
        interactionUI = CharacterManager.Instance.Player.interactionUI;
    }

    public void OnInteraction()
    {
        interactionUI.SetActive(true);
        interactionUI.Set(ItemData);
    }
}