using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private BuildingData buildingData;
    private GameObject previewInstance;     //�̸����� �ν��Ͻ�
    public Color validColor = Color.white;  //��ġ ���� ����
    public Color invalidColor = Color.red;  //��ġ �Ұ��� ����
    public LayerMask groundLayer;

    public float maxDistance;               //��ġ �Ÿ�

    private bool canBuild = false;
    private List<Collider> collidersList = new List<Collider>();

    private BuildingManager buildingManager;
    
    void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    public void StartPreview(BuildingData building)
    {
        //�̸����� ������Ʈ ����
        if (previewInstance != null)
        {
            buildingData = null;
            Destroy(previewInstance);
        }

        buildingData = building;

        previewInstance = Instantiate(buildingData.buildPrefab);
        previewInstance.GetComponent<Renderer>().material.color = validColor;

        Collider previewCollider = previewInstance.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.isTrigger = true;
        }

        previewInstance.SetActive(true);
    }


    void Update()
    {
        Vector3 targetPosition = GetTargetPosition();
        previewInstance.transform.position = targetPosition;

        canBuild = CheckBuildable(targetPosition);
        SetPreviewColor(canBuild);
    }

    //��ġ ��ġ ��ȯ
    private Vector3 GetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxDistance, groundLayer))
        {
            Vector3 hitPoint = hit.point;

            float buildingHeight = previewInstance.GetComponent<Collider>().bounds.extents.y;
            hitPoint.y = hit.point.y + buildingHeight;

            return hitPoint;
        }

        return Vector3.zero;
    }

    //��ġ ���� ���� Ȯ��
    private bool CheckBuildable(Vector3 targetPosition)
    {
        if (collidersList.Count > 0 || !CheckSlope(targetPosition))
        {
            canBuild = false;
        }
        else
        {
            canBuild = true;
        }

        return canBuild;
    }
    
    //�̸����� ���� ����
    private void SetPreviewColor(bool canBuild)
    {
        previewInstance.GetComponent<Renderer>().material.color = canBuild ? validColor : invalidColor;
    }

    //�ٴ� ���� üũ
    private bool CheckSlope(Vector3 position)
    {
        float slope = Terrain.activeTerrain.terrainData.GetSteepness(position.x / Terrain.activeTerrain.terrainData.size.x, position.z / Terrain.activeTerrain.terrainData.size.z);
        return slope > 15f;
    }

    //��ġ �� �̸����� ��Ȱ��ȭ
    private void InstallBuilding(Vector3 targetPosition)
    {
        buildingManager.BuildBuilding(buildingData, targetPosition);
        previewInstance.SetActive(false);
    }

    //��ġ�� ������Ʈ üũ
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != groundLayer)
        {
            collidersList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != groundLayer)
        {
            collidersList.Remove(other);
        }
    }
}
