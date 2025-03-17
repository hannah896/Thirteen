using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertTerrainTree : MonoBehaviour
{
    public Terrain terrain; // Terrain 객체
    public GameObject treePrefab; // 변환할 나무 프리팹

    private void OnValidate()
    {
        terrain = GetComponent<Terrain>();
    }
    void Start()
    {
        ConvertTrees();
    }

    void ConvertTrees()
    {
        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] trees = terrainData.treeInstances;
        List<GameObject> newTrees = new List<GameObject>();

        for (int i = 0; i < trees.Length; i++)
        {
            TreeInstance tree = trees[i];

            // Terrain 좌표를 월드 좌표로 변환
            Vector3 worldPos = ConvertTreePositionToWorld(tree.position);

            // 나무 프리팹 생성
            GameObject newTree = Instantiate(treePrefab, worldPos, Quaternion.identity);
            newTree.tag = "Tree"; // 태그 추가

            // Collider가 없으면 추가
            if (newTree.GetComponent<Collider>() == null)
            {
                newTree.AddComponent<BoxCollider>();
            }

            newTrees.Add(newTree);
        }

        // 기존 Terrain 나무 제거
        terrainData.treeInstances = new TreeInstance[0];

        Debug.Log($"변환 완료: {newTrees.Count}개의 나무가 GameObject로 변경됨.");
    }

    // Terrain 좌표를 월드 좌표로 변환하는 함수
    Vector3 ConvertTreePositionToWorld(Vector3 terrainPos)
    {
        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 terrainBasePos = terrain.transform.position;

        return new Vector3(
            terrainBasePos.x + terrainPos.x * terrainSize.x,
            terrainBasePos.y + terrainPos.y * terrainSize.y,
            terrainBasePos.z + terrainPos.z * terrainSize.z
        );
    }
}
