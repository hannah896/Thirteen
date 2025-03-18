using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ResourceAutoMaker : MonoBehaviour
{
    [Header("ResourceInfo")]
    public List<GameObject> rock = new List<GameObject>();
    public List<GameObject> tree = new List<GameObject>();
    public List<GameObject> bush = new List<GameObject>();

    public GameObject resource;

    [Header("GroundInfo")]
    NavMeshAgent agent;
    GameObject terrain;
    Collider _collider;
    float minX;
    float maxX;
    float PosY;
    float minZ;
    float maxZ;

    [Header("BoxInfo")]
    public List<GameObject> box = new List<GameObject>();
    
    private void OnValidate()
    {
        if (terrain == null) terrain = GameObject.Find("Terrain");
        if (_collider == null) _collider = terrain.GetComponent<Collider>();
        if (agent == null) GetComponent<NavMeshAgent>();
        if (resource == null)
        {
            resource = new GameObject("Resources");
            resource.transform.position = Vector3.zero;
        }
    }

    private void Awake()
    {
        minX = _collider.bounds.min.x;
        minZ = _collider.bounds.min.z;
        maxX = _collider.bounds.max.x;
        maxZ = _collider.bounds.max.z;
        PosY = _collider.bounds.max.y;
    }

    private void Start()
    {
        for (int i = 0; i < 200; i++)
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

        for (int i = 0; i < 33; i++)
        {
            foreach(GameObject bx in box)
            {
                //상자 생성
                Instantiate
                    (
                        bx,
                        RandomPosition(),
                        Quaternion.identity,
                        resource.transform
                    );
            }
        }

        terrain.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    //땅의 위치중 랜덤 위치 지정
    private Vector3 RandomPosition()
    {
        for (int i = 0; i < 30; i++) // 30번 시도
        {
            //땅의 너비에서 테두리의 너비 5정도를 제외시킴
            float x = Random.Range(minX + 5, maxX - 5);
            float z = Random.Range(minZ + 5, maxZ - 5);
            
            //랜덤좌표 생성
            Vector3 point = new Vector3(x, PosY, z);

            if (NavMesh.SamplePosition(point, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return new Vector3((minX + maxX) / 2f, PosY, (minZ + maxZ) / 2f);
    }
}