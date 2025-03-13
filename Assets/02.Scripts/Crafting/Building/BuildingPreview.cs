using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private BuildingManager buildingManager;
    private BuildingData buildingData;
    private BuildingObject buildingObject;
    private GameObject previewInstance;     //미리보기 인스턴스
    public Color validColor = Color.green;  //설치 가능 색상
    public Color invalidColor = Color.red;  //설치 불가능 색상
    public LayerMask groundLayer;

    public float maxDistance;               //설치 거리
    private Quaternion previewRotation = Quaternion.identity;

    private bool canBuild = false;

    //테스트
    public Camera cam;
    
    void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    public void StartPreview(BuildingData building)
    {
        //미리보기 오브젝트 셋팅
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
        //미리보기 위치 표시
        if (previewInstance == null) return;

        Vector3 targetPosition = GetTargetPosition();
        previewInstance.transform.position = targetPosition;

        canBuild = CheckBuildable(targetPosition);
        SetPreviewColor(canBuild);

        //InputSystem으로 변경 예정
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

    //설치 위치 반환
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

    //설치 가능 여부 확인
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
    
    //미리보기 색상 변경
    private void SetPreviewColor(bool canBuild)
    {
        previewInstance.GetComponent<Renderer>().material.color = canBuild ? validColor : invalidColor;
    }

    //바닥 경사면 체크
    private bool CheckSlope(Vector3 position)
    {
        float slope = Terrain.activeTerrain.terrainData.GetSteepness(position.x / Terrain.activeTerrain.terrainData.size.x, position.z / Terrain.activeTerrain.terrainData.size.z);
        return slope > 15f || slope < -15;
    }

    //설치 후 미리보기 삭제
    private void InstallBuilding(Vector3 targetPosition)
    {
        buildingManager.BuildBuilding(buildingData, targetPosition, previewRotation);
        Destroy(previewInstance);
    }
}
