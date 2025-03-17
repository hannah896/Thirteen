using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public BuildingManager buildingManager;
    private BuildingData buildingData;
    private BuildingObject buildingObject;
    private GameObject previewInstance;     //미리보기 인스턴스
    private Color validColor = Color.green;  //설치 가능 색상
    private Color invalidColor = Color.red;  //설치 불가능 색상

    public LayerMask groundLayer;
    public float maxDistance;               //설치 거리
    public float maxSlopeAngle = 10f;       //설치 가능 경사

    private Quaternion previewRotation;   //미리보기 회전값
    private bool canBuild = false;

    //테스트
    public Camera cam;
    
    void Start()
    {
        //buildingManager = FindObjectOfType<BuildingManager>();
    }

    public void StartPreview(BuildingData building)
    {
        //미리보기 오브젝트 셋팅
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }

        buildingData = building;
        previewInstance = Instantiate(buildingData.buildPrefab);
        buildingObject = previewInstance.GetComponent<BuildingObject>();

        previewRotation = previewInstance.transform.rotation;

        SetupPreviewColliders();
        previewInstance.SetActive(true);
    }

    void Update()
    {
        //미리보기 위치 표시
        if (previewInstance == null) return;

        Vector3 targetPosition = GetTargetPosition();
        previewInstance.transform.position = targetPosition;

        canBuild = CheckBuildable(targetPosition) && CheckSlope();
        SetPreviewColor(canBuild);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(previewInstance);
        }
    }

    //미리보기 isTrigger 세팅
    private void SetupPreviewColliders()
    {
        Collider[] colliders = previewInstance.GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            if (collider is MeshCollider mesh)
            {
                mesh.convex = true;
            }
            collider.isTrigger = true;
        }
    }

    //설치 위치 반환
    private Vector3 GetTargetPosition()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if(Physics.Raycast(ray, out RaycastHit hit, maxDistance, groundLayer))
        {
            Vector3 hitPoint = hit.point;

            return hitPoint;
        }

        return Vector3.zero;
    }

    //설치 가능 여부 확인
    private bool CheckBuildable(Vector3 targetPosition)
    {
        if (buildingObject.colliderList.Count == 0) return true;
        else return false;
    }
    
    //미리보기 색상 변경
    private void SetPreviewColor(bool canBuild)
    {
        previewInstance.GetComponent<Renderer>().material.color = canBuild ? validColor : invalidColor;
    }

    //바닥 경사면 체크
    private bool CheckSlope()
    {
        Vector3[] checkPoint = new Vector3[4];
        Bounds bounds = previewInstance.GetComponent<Renderer>().bounds;

        checkPoint[0] = new Vector3(bounds.min.x, bounds.center.y, bounds.min.z);
        checkPoint[1] = new Vector3(bounds.min.x, bounds.center.y, bounds.max.z);
        checkPoint[2] = new Vector3(bounds.max.x, bounds.center.y, bounds.min.z);
        checkPoint[3] = new Vector3(bounds.max.x, bounds.center.y, bounds.max.z);

        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach(Vector3 point in checkPoint)
        {
            if(Physics.Raycast(point + Vector3.up, Vector3.down, out RaycastHit hit, 10f, groundLayer))
            {
                minY = Mathf.Min(minY, hit.point.y);
                maxY = Mathf.Max(maxY, hit.point.y);
            }
        }

        float heightDifference = maxY - minY;
        float slopAngle = Mathf.Atan(heightDifference / bounds.size.x) * Mathf.Rad2Deg;

        return slopAngle <= maxSlopeAngle;
    }

    public void PreviewRotate(Vector2 scroll)
    {
        if (scroll.y > 0)
        {
            previewRotation *= Quaternion.Euler(0, -90f, 0);
            previewInstance.transform.rotation = previewRotation;
        }
        else
        {
            previewRotation *= Quaternion.Euler(0, 90f, 0);
            previewInstance.transform.rotation = previewRotation;
        }
    }

    public void ConfirmBuild()
    {
        if (canBuild)
        {
            InstallBuilding(previewInstance.transform.position);
        }
    }

    //설치 후 미리보기 삭제
    private void InstallBuilding(Vector3 targetPosition)
    {
        CharacterManager.Instance.Player.controller.isBuildMode = false;
        buildingManager.BuildBuilding(buildingData, targetPosition, previewRotation);
        Destroy(previewInstance);
    }
}
