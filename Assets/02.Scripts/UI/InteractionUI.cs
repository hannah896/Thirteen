using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionName;
    [SerializeField] private TextMeshProUGUI interactionDescription;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Set(ItemData data)
    {
        interactionName.text = data.itemName;
        interactionDescription.text = data.itemDescription;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
