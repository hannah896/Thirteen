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

    private void Start()
    {
        buildingManager = GetComponent<BuildingManager>();
        ClearSelectedBuildingWindow();
        CreateBuildingList();
        SelectBuilding(0);
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
        if (slots[index].building == null || slots[index].building == selectedBuilding) return;

        selectedBuilding = slots[index].building;

        icon.sprite = selectedBuilding.icon;
        selectedBuildingName.text = selectedBuilding.displayName;
        selectedBuildingDescription.text = selectedBuilding.description;
        selectedRequiredName.text = string.Empty;
        selectedRequiredAmount.text = string.Empty;

        for (int i = 0; i < selectedBuilding.requiredResources.Count; i++)
        {
            selectedRequiredName.text += selectedBuilding.requiredResources[i].resource.name + "\n";
            selectedRequiredAmount.text += selectedBuilding.requiredResources[i].amount.ToString() + "\n";
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
        if (selectedBuilding == null) return;

        buildingWindow.SetActive(false);
        buildingManager.OnSelectBuilding(selectedBuilding);
    }

    //UI 종료 버튼
    public void OnExitButton()
    {
        buildingWindow.SetActive(false);
    }
}