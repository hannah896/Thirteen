using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceAutoMaker : MonoBehaviour
{
    List<GameObject> rock = new List<GameObject>();
    List<GameObject> tree = new List<GameObject>();
    List<GameObject> bush = new List<GameObject>();

    GameObject resource;

    private void Start()
    {
        resource = new GameObject("Resources");
        GameObject resource1;
        GameObject resource2;
        GameObject resource3;
        for (int i = 0; i < 30; i++)
        {
            resource1 = Instantiate(rock[Random.Range(0, rock.Count)], resource.transform);
            resource2 = Instantiate(tree[Random.Range(0, rock.Count)], resource.transform);
            resource3 = Instantiate(bush[Random.Range(0, rock.Count)], resource.transform);
        }
    }
}
