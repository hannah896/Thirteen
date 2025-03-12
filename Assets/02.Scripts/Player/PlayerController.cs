using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraContainer; // 메인 카메라 부모
    [SerializeField] private Transform character;       // 캐릭터 매쉬 트랜스폼

    private Rigidbody rigid;        // 캐릭터의 리지드 바디

    private Vector2 mouseDelta;     // 마우스 움직임 변화량
    private Vector2 inputDir;       // 입력받은 방향

    [SerializeField] private float mouseInsensity = 0.1f;
    [SerializeField] private float minXRot;
    [SerializeField] private float maxXRot;
    private float camXRot;          // 카메라 x 회전 값

    [SerializeField] private float lerpSpeed;      // 캐릭터 회전 속도

    [SerializeField] private float walkSpeed;       // 걷는 속도
    [SerializeField] private float runSpeed;        // 달리는 속도
    [SerializeField] private bool isRun;            // 달리는 키 입력이 되었는지 확인

    private Animator animator;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        Vector3 moveDir = cameraContainer.forward * inputDir.y + cameraContainer.right * inputDir.x;
        // 달리기 키를 입력 받았다면 뛰는 속도로 적용
        float speed = isRun ? runSpeed : walkSpeed;
        moveDir *= speed;
        moveDir.y = rigid.velocity.y;

        if(inputDir.magnitude > 0)
        {
            Vector3 lookDir = new Vector3(moveDir.x, 0, moveDir.z);
            character.forward = Vector3.Lerp(character.forward, lookDir, Time.deltaTime * lerpSpeed);
        }

        rigid.velocity = moveDir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        animator.SetFloat("InputDir", inputDir.magnitude);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
        Debug.Log(mouseDelta);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            isRun = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
        }
        animator.SetBool("IsRun", isRun);
    }
}
