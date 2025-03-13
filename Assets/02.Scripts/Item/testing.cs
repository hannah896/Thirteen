using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        // Vertical: 위아래 방향 입력 (W/S 또는 위쪽/아래쪽 화살표)
        float vertical = Input.GetAxis("Vertical");

        // 캐릭터 이동 처리
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
