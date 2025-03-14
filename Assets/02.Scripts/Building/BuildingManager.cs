using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<BuildingData> buildingList;
    public BuildingPreview buildingPreview;
    
    public bool CanBuild(BuildingData building)
    {
        //�κ��丮�� �ʿ��� �������� ������ ������� üũ�ϰ� return �ϴ� �޼���
        //�ʿ� �Ű������� building.requiredResources
        return true;
    }

    public void BuildBuilding(BuildingData building, Vector3 position, Quaternion rotation)
    {
        //�κ��丮���� ��� ���� �� �ǹ� ����
        Instantiate(building.buildPrefab, position, rotation);
    }

    public void OnSelectBuilding(BuildingData building)
    {
        //�̸����� ȣ��
        buildingPreview.StartPreview(building);
    }
}