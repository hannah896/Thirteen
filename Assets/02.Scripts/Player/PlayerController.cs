using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigid;        // 캐릭터의 리지드 바디

    private Vector2 inputDir;       // 입력받은 방향

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector3(inputDir.x, rigid.velocity.y, inputDir.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }
}
