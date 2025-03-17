using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BuildingManager : MonoBehaviour
{
    public List<BuildingData> buildingList;
    public BuildingPreview buildingPreview;
    
    public bool CanBuild(BuildingData building)
    {
        return InventoryManager.Instance.consumeInventory.HasRequiredResources(building.requiredResources);
    }

    public void BuildBuilding(BuildingData building, Vector3 position, Quaternion rotation)
    {
        //인벤토리에서 재료 차감 후 건물 생성
        if (InventoryManager.Instance.consumeInventory.ConsumeResources(building.requiredResources))
        {
            Instantiate(building.buildPrefab, position, rotation);
        }
    }

    public void OnSelectBuilding(BuildingData building)
    {
        //미리보기 호출
        buildingPreview.StartPreview(building);
    }
}