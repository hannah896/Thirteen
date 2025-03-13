using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private BuildingManager buildingManager;
    private BuildingData buildingData;
    private BuildingObject buildingObject;
    private GameObject previewInstance;     //�̸����� �ν��Ͻ�
    public Color validColor = Color.green;  //��ġ ���� ����
    public Color invalidColor = Color.red;  //��ġ �Ұ��� ����
    public LayerMask groundLayer;

    public float maxDistance;               //��ġ �Ÿ�
    private Quaternion previewRotation = Quaternion.identity;

    private bool canBuild = false;

    //�׽�Ʈ
    public Camera cam;
    
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
        buildingObject = previewInstance.GetComponent<BuildingObject>();

        Collider previewCollider = previewInstance.GetComponent<Collider>();
        if (previewCollider != null)
        {
            if (previewCollider is MeshCollider mesh)
            {
                mesh.convex = true;
                mesh.isTrigger = true;
            }
            else
                previewCollider.isTrigger = true;
        }

        previewInstance.SetActive(true);
    }

    void Update()
    {
        //�̸����� ��ġ ǥ��
        if (previewInstance == null) return;

        Vector3 targetPosition = GetTargetPosition();
        previewInstance.transform.position = targetPosition;

        canBuild = CheckBuildable(targetPosition);
        SetPreviewColor(canBuild);

        //InputSystem���� ���� ����
        if (Input.GetKeyDown(KeyCode.R) && canBuild)
        {
            InstallBuilding(targetPosition);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(previewInstance);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            previewRotation *= Quaternion.Euler(0, -90f, 0);
            previewInstance.transform.rotation = previewRotation;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            previewRotation *= Quaternion.Euler(0, 90f, 0);
            previewInstance.transform.rotation = previewRotation;
        }
    }

    //��ġ ��ġ ��ȯ
    private Vector3 GetTargetPosition()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxDistance, groundLayer))
        {
            Vector3 hitPoint = hit.point;

            return hitPoint;
        }

        return Vector3.zero;
    }

    //��ġ ���� ���� Ȯ��
    private bool CheckBuildable(Vector3 targetPosition)
    {
        if (buildingObject.colliderList.Count > 0)
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
        return slope > 15f || slope < -15;
    }

    //��ġ �� �̸����� ����
    private void InstallBuilding(Vector3 targetPosition)
    {
        buildingManager.BuildBuilding(buildingData, targetPosition, previewRotation);
        Destroy(previewInstance);
    }
}
