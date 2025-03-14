using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICrafting : MonoBehaviour
{
    public CraftingSlot[] slots;

    public GameObject craftingWindow;
    public GameObject craftingListPrefab;
    public Transform contentParent;

    [Header("Select Item")]
    public Sprite icon;
    public TextMeshProUGUI selectedItemName;        //아이템 이름
    public TextMeshProUGUI selectedItemDescription; //아이템 설명
    public TextMeshProUGUI selectedRequiredName;        //필요 재료
    public TextMeshProUGUI selectedRequiredAmount;      //필요 수량
    public Button buildButton;

    private CraftingManager craftingManager;
    private ItemData selectedItem;

    private void Start()
    {
        craftingManager = GetComponent<CraftingManager>();
        ClearSelectedCraftingWindow();
        CreateItemList();
        SelectItem(0);
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
    void ClearCraftList()
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

        ClearCraftList();
    }

    //List 선택 세팅
    public void SelectItem(int index)
    {
        if (slots[index].Item == null) return;

        selectedItem = slots[index].Item;

        icon = selectedItem.Icon;
        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.itemDescription;
        //for (int i = 0; i < selectedItem.requiredResources.Count; i++)
        //{
        //    selectedRequiredName.text += selectedItem.requiredResources[i].resource + "\n";
        //    selectedRequiredAmount.text += selectedItem.requiredResources[i].amount.ToString() + "\n";
        //}

        if (craftingManager.CanCraft(selectedItem))
        {
            buildButton.enabled = true;
            buildButton.image.color = Color.white;
        }
        else
        {
            buildButton.enabled = false;
            buildButton.image.color = Color.gray;
        }
    }

    //제작 버튼
    public void OnCraftingButton()
    {
        if (selectedItem == null) return;

        craftingWindow.SetActive(false);
        craftingManager.CraftItem(selectedItem);
    }

    //UI 종료 버튼
    public void OnExitButton()
    {
        craftingWindow.SetActive(false);
    }
}
