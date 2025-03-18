using System.Collections.Generic;
using UnityEngine;

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
        bool isBuild = true;

        //인벤토리에서 재료 차감 후 건물 생성
        if (building.requiredResources.Count > 0)
        {
            isBuild = InventoryManager.Instance.consumeInventory.ConsumeResources(building.requiredResources);
        }

        if (isBuild)
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