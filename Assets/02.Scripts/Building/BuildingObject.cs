using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public List<Collider> colliderList = new List<Collider>();
    public LayerMask layerMask;


    //겹치는 오브젝트 체크
    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value == (1 << other.gameObject.layer)))
        {
            return;
        }
        else if (other.gameObject.name == "Floor")
        {
            return;
        }

        colliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value == (1 << other.gameObject.layer)))
        {
            return;
        }
        else if (other.gameObject.name == "Floor")
        {
            return;
        }

        colliderList.Remove(other);
    }
}
