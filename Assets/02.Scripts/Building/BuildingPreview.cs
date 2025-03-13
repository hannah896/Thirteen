using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private BuildingData buildingData;
    private GameObject previewInstance;     //미리보기 인스턴스
    public Color validColor = Color.white;  //설치 가능 색상
    public Color invalidColor = Color.red;  //설치 불가능 색상
    public LayerMask groundLayer;

    public float maxDistance;               //설치 거리

    private bool canBuild = false;
    private List<Collider> collidersList = new List<Collider>();

    private BuildingManager buildingManager;
    
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

    //설치 위치 반환
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

    //설치 가능 여부 확인
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
    
    //미리보기 색상 변경
    private void SetPreviewColor(bool canBuild)
    {
        previewInstance.GetComponent<Renderer>().material.color = canBuild ? validColor : invalidColor;
    }

    //바닥 경사면 체크
    private bool CheckSlope(Vector3 position)
    {
        float slope = Terrain.activeTerrain.terrainData.GetSteepness(position.x / Terrain.activeTerrain.terrainData.size.x, position.z / Terrain.activeTerrain.terrainData.size.z);
        return slope > 15f;
    }

    //설치 후 미리보기 비활성화
    private void InstallBuilding(Vector3 targetPosition)
    {
        buildingManager.BuildBuilding(buildingData, targetPosition);
        previewInstance.SetActive(false);
    }

    //겹치는 오브젝트 체크
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
