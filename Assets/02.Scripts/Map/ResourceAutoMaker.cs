using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector.Libs;

public class ResourceAutoMaker : MonoBehaviour
{
    [Header("ResourceInfo")]
    public List<GameObject> rock = new List<GameObject>();
    public List<GameObject> tree = new List<GameObject>();
    public List<GameObject> bush = new List<GameObject>();

    public GameObject resource;

    [Header("GroundInfo")]
    GameObject terrain;
    Collider collider;
    float minX;
    float maxX;
    float PosY;
    float minZ;
    float maxZ;
    
    private void OnValidate()
    {
        terrain = GameObject.Find("Terrain");
        collider = terrain.GetComponent<Collider>();
    }

    private void Awake()
    {
        if (resource == null) resource = new GameObject("Resources");
        minX = collider.bounds.min.x;
        minZ = collider.bounds.min.z;
        maxX = collider.bounds.max.x;
        maxZ = collider.bounds.max.z;
        PosY = collider.bounds.max.y;
    }

    private void Start()
    {
        for (int i = 0; i < 500; i++)
        {
            //바위 생성
            Instantiate
                (
                    rock[Random.Range(0, rock.Count)],
                    RandomPosition(),
                    Quaternion.identity,
                    resource.transform
                );

            //나무 생성
            Instantiate
                (
                    tree[Random.Range(0, tree.Count)],
                    RandomPosition(),
                    Quaternion.identity,
                    resource.transform
                );
            
            //덤불 생성
            Instantiate
                (
                    bush[Random.Range(0, bush.Count)],
                    RandomPosition(),
                    Quaternion.identity,
                    resource.transform
                );
        }
    }

    //땅의 위치중 랜덤 위치 지정
    private Vector3 RandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        Vector3 result = new Vector3(x, PosY, z);
        return result;
    }
}
