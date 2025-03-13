using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        // Vertical: ���Ʒ� ���� �Է� (W/S �Ǵ� ����/�Ʒ��� ȭ��ǥ)
        float vertical = Input.GetAxis("Vertical");

        // ĳ���� �̵� ó��
        Vector3 movement = new Vector3(horizontal, 0, vertical) * Time.deltaTime;
        transform.Translate(movement);

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
