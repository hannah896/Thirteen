using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public List<Collider> colliderList = new List<Collider>();
    public LayerMask layerMask;

    //��ġ�� ������Ʈ üũ
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
