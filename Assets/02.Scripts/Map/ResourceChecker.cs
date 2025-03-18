using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceChecker : MonoBehaviour
{
    public Collider _collider;
    int _count = 0;

    private void OnValidate()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _count++;
        Debug.Log(_count);
    }
}
