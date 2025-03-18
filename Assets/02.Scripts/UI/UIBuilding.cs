using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilding : MonoBehaviour
{
    public BuildingSlot[] slots;

    [Header("UI Elements")]
    public GameObject buildingWindow;       //건축 UI 윈도우
    public GameObject buildingListPrefab;   //ScrollView List Prefab
    public Transform contentParent;         //ScrollView List가 들어갈 부모 Object
    public Button buildButton;

    [Header("Select Building Info")]
    public TextMeshProUGUI selectedBuildingName;        //건축물 이름
    public TextMeshProUGUI selectedBuildingDescription; //건축물 설명
    public TextMeshProUGUI selectedRequiredName;        //필요 재료
    public TextMeshProUGUI selectedRequiredAmount;      //필요 수량
    public Image icon;

    private BuildingManager buildingManager;
    private BuildingData selectedBuilding;
    private PlayerController controller;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;

        controller.building += Toggle;

        buildingWindow.SetActive(false);

        buildingManager = GetComponent<BuildingManager>();

        ClearSelectedBuildingWindow();
        CreateBuildingList();
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            buildingWindow.SetActive(false);
        }
        else
        {
            SelectBuilding(0);
            buildingWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return buildingWindow.activeInHierarchy;
    }

    //건축 List 생성
    void CreateBuildingList()
    {

        foreach (var data in buildingManager.buildingList)
        {
            GameObject newList = Instantiate(buildingListPrefab, contentParent);
            newList.GetComponentInChildren<TextMeshProUGUI>().text = data.displayName;
        }

        slots = new BuildingSlot[contentParent.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = contentParent.GetChild(i).GetComponent<BuildingSlot>();
            slots[i].building = buildingManager.buildingList[i];
            slots[i].index = i;
            slots[i].uiBuilding = this;
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
    void ClearSelectedBuildingWindow()
    {
        selectedBuildingName.text = string.Empty;
        selectedBuildingDescription.text = string.Empty;
        selectedRequiredName.text = string.Empty;
        selectedRequiredAmount.text = string.Empty;

        ClearList();
    }

    //선택 시 UI 업데이트
    public void SelectBuilding(int index)
    {
        if (slots[index].building == null) return;

        selectedBuilding = slots[index].building;

        icon.sprite = selectedBuilding.icon;
        selectedBuildingName.text = selectedBuilding.displayName;
        selectedBuildingDescription.text = selectedBuilding.description;
        selectedRequiredName.text = string.Empty;
        selectedRequiredAmount.text = string.Empty;

        for (int i = 0; i < selectedBuilding.requiredResources.Count; i++)
        {
            int resourceQuantity = InventoryManager.Instance.consumeInventory.GetItemQuantity(selectedBuilding.requiredResources[i]);

            selectedRequiredName.text += selectedBuilding.requiredResources[i].resource.itemName + "\n";
            selectedRequiredAmount.text += $"({resourceQuantity} / {selectedBuilding.requiredResources[i].amount}) \n";
        }

        if (buildingManager.CanBuild(selectedBuilding))
        {
            buildButton.interactable = true;
        }
        else
        {
            buildButton.interactable = false;
        }
    }

    //건축 버튼
    public void OnBuildingButton()
    {
        AudioManager.instance.PlaySFX("Button");
        if (selectedBuilding == null) return;

        controller.isBuildMode = true;
        controller.CursorVisible();

        buildingWindow.SetActive(false);
        buildingManager.OnSelectBuilding(selectedBuilding);
    }

    //UI 종료 버튼
    public void OnExitButton()
    {
        AudioManager.instance.PlaySFX("Button");
        controller.CursorVisible();
        buildingWindow.SetActive(false);
    }
}