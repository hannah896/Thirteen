using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraContainer; // 메인 카메라 부모

    private Rigidbody rigid;        // 캐릭터의 리지드 바디

    private Vector2 mouseDelta;     // 마우스 움직임 변화량
    private Vector2 inputDir;       // 입력받은 방향

    [SerializeField] private float mouseInsensity = 0.1f;
    [SerializeField] private float minXRot;
    [SerializeField] private float maxXRot;
    private float camXRot;          // 카메라 x 회전 값

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }
    
    private void Look()
    {
        camXRot += mouseDelta.y * mouseInsensity;
        camXRot = Mathf.Clamp(camXRot, minXRot, maxXRot);

        float yRot = cameraContainer.localEulerAngles.y + mouseDelta.x * mouseInsensity;

        cameraContainer.localEulerAngles = new Vector3(-camXRot, yRot, 0);
    }

    private void Move()
    {
        rigid.velocity = new Vector3(inputDir.x, rigid.velocity.y, inputDir.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
        Debug.Log(mouseDelta);
    }
}
