using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilding : MonoBehaviour
{
    public BuildingSlot[] slots;

    public GameObject buildingWindow;
    public GameObject buildingListPrefab;
    public Transform contentParent;

    [Header("Select Item")]
    public Image icon;
    public TextMeshProUGUI selectedBuildingName;
    public TextMeshProUGUI selectedBuildingDescription;
    public TextMeshProUGUI selectedRequiredName;
    public TextMeshProUGUI selectedRequiredAmount;

    private BuildingManager buildingManager;
    private BuildingData selectedBuilding;
    private int selectedIndex;

    private void Start()
    {
        buildingManager = GetComponent<BuildingManager>();
        ClearSelectedBuildingWindow();
        CreateBuildingList();
        SelectBuilding(0);
    }

    void CreateBuildingList()
    {

        foreach(var data in buildingManager.buildingList)
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

    void ClearCraftList()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

    void ClearSelectedBuildingWindow()
    {
        selectedBuildingName.text = string.Empty;
        selectedBuildingDescription.text = string.Empty;
        selectedRequiredName.text = string.Empty;
        selectedRequiredAmount.text = string.Empty;

        ClearCraftList();
    }

    public void SelectBuilding(int index)
    {
        if (slots[index].building == null) return;

        selectedBuilding = slots[index].building;
        selectedIndex = index;

        icon = selectedBuilding.icon;
        selectedBuildingName.text = selectedBuilding.displayName;
        selectedBuildingDescription.text = selectedBuilding.description;
        for (int i = 0; i < selectedBuilding.requiredResources.Count; i++)
        {
            selectedRequiredName.text += selectedBuilding.requiredResources[i].resource + "\n";
            selectedRequiredAmount.text += selectedBuilding.requiredResources[i].amount.ToString() + "\n";
        }
    }

    public void OnBuildingButton()
    {
        if (selectedBuilding == null) return;

        buildingWindow.SetActive(false);
        buildingManager.OnSelectBuilding(selectedBuilding);
    }

    public void OnExitButton()
    {
        buildingWindow.SetActive(false);
    }
}