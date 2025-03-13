using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<BuildingData> buildingList;
    public BuildingPreview buildingPreview;
    
    public bool CanBuild(BuildingData building)
    {
        //인벤토리에 필요한 아이템의 갯수가 충분한지 체크하고 return 하는 메서드
        //필요 매개변수는 building.requiredResources
        return true;
    }

    public void BuildBuilding(BuildingData building, Vector3 position, Quaternion rotation)
    {
        //인벤토리에서 재료 삭제 후 건물 짓기
        Instantiate(building.buildPrefab, position, rotation);
    }

    public void OnSelectBuilding(BuildingData building)
    {
        //미리보기 호출
        buildingPreview.StartPreview(building);
    }
}