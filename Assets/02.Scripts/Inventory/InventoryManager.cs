using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;

    public EquipInventory equipInventory;     // 장비용 인벤토리
    public ConsumeInventory consumeInventory;   // 소비, 재료 용 인벤토리

    public static InventoryManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObject("InventoryManager").AddComponent<InventoryManager>();

            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            if(_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        CharacterManager.Instance.Player.inventory += Toggle;

        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

    }

    public bool IsOpen()
    {
        return gameObject.activeInHierarchy;
    }
}
