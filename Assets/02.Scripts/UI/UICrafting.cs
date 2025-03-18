using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICrafting : MonoBehaviour
{
    public CraftingSlot[] slots;

    [Header("UI Elements")]
    public GameObject craftingWindow;
    public GameObject craftingListPrefab;
    public Transform contentParent;
    public Button craftButton;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;        //아이템 이름
    public TextMeshProUGUI selectedItemDescription; //아이템 설명
    public TextMeshProUGUI selectedRequiredName;        //필요 재료
    public TextMeshProUGUI selectedRequiredAmount;      //필요 수량
    public Image icon;

    private CraftingManager craftingManager;
    private ItemData selectedItem;
    private PlayerController controller;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;

        controller.crafting += Toggle;

        craftingWindow.SetActive(false);

        craftingManager = GetComponent<CraftingManager>();

        ClearSelectedCraftingWindow();
        CreateItemList();
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            craftingWindow.SetActive(false);
        }
        else
        {
            SelectItem(0);
            craftingWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return craftingWindow.activeInHierarchy;
    }

    //건축 List 생성
    void CreateItemList()
    {

        foreach (var data in craftingManager.itemList)
        {
            GameObject newList = Instantiate(craftingListPrefab, contentParent);
            newList.GetComponentInChildren<TextMeshProUGUI>().text = data.itemName;
        }

        slots = new CraftingSlot[contentParent.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = contentParent.GetChild(i).GetComponent<CraftingSlot>();
            slots[i].Item = craftingManager.itemList[i];
            slots[i].index = i;
            slots[i].uICrafting = this;
        }
    }

    //List 초기화
    void ClearList()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

    //선택 화면 초기화
    void ClearSelectedCraftingWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedRequiredName.text = string.Empty;
        selectedRequiredAmount.text = string.Empty;

        ClearList();
    }

    //선택 시 UI 업데이트
    public void SelectItem(int index)
    {
        if (slots[index].Item == null || slots[index].Item == selectedItem) return;

        selectedItem = slots[index].Item;

        icon.sprite = selectedItem.Icon;
        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.itemDescription;
        selectedRequiredName.text = string.Empty;
        selectedRequiredAmount.text = string.Empty;

        for (int i = 0; i < selectedItem.cost.Count; i++)
        {
            int resourceQuantity = InventoryManager.Instance.consumeInventory.GetItemQuantity(selectedItem.cost[i]);

            selectedRequiredName.text += selectedItem.cost[i].resource.itemName + "\n";
            selectedRequiredAmount.text += $"({resourceQuantity} / {selectedItem.cost[i].amount}) \n";
        }

        if (craftingManager.CanCraft(selectedItem))
        {
            craftButton.interactable = true;
        }
        else
        {
            craftButton.interactable = false;
        }
    }

    //제작 버튼
    public void OnCraftingButton()
    {
        if (selectedItem == null) return;

        controller.CursorVisible();

        craftingWindow.SetActive(false);
        craftingManager.CraftItem(selectedItem);
    }

    //UI 종료 버튼
    public void OnExitButton()
    {
        controller.CursorVisible();
        craftingWindow.SetActive(false);
    }
}
