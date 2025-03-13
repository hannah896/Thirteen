using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public List<Collider> colliderList = new List<Collider>();
    public LayerMask layerMask;

    //겹치는 오브젝트 체크
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerMask)
        {
            colliderList.Add(other);
        }
        Debug.Log(colliderList.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerMask)
        {
            colliderList.Remove(other);
        }
        Debug.Log(colliderList.Count);
    }
}
