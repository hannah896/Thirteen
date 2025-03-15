using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 4f, LayerMask.GetMask("Resource")))
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                hit.transform.gameObject.GetComponent<Resource>().MakingResource();
            }
        }
    }
}
