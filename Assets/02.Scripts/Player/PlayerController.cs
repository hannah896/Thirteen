using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
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

    private bool isJump;                        // 점프 상태 확인
    [SerializeField] private float jumpForce;   // 점프 파워

    [SerializeField] private LayerMask groundMask;  // 땅을 표시하는 Layer

    private bool isAttack;                      // 공격 상태 확인

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        if (isJump) return;

        if(isAttack)
        {
            // 공격 시 rigid의 Velocity를 0으로 만들어 줌
            rigid.velocity = Vector3.zero;
            return;
        }

        Vector3 moveDir = cameraContainer.forward * inputDir.y + cameraContainer.right * inputDir.x;
        // 달리기 키를 입력 받았다면 뛰는 속도로 적용
        float speed = isRun ? runSpeed : walkSpeed;
        moveDir *= speed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;

        if (inputDir.magnitude > 0)
        {
            moveDir.Normalize();
            Vector3 lookDir = new Vector3(moveDir.x, 0, moveDir.z);
            character.forward = Vector3.Lerp(character.forward, lookDir, Time.deltaTime * lerpSpeed);
        }
    }

    private void Jump()
    {
        isJump = true;
        animator.SetTrigger("Jump");
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // 바닥에 캐릭터가 닿아있는지 확인하는 함수
    private bool IsGround()
    {
        // 우선 캐릭터 아래 방향으로 발사되는 Ray 4개를 준비한다.
        float rayDistance = 0.2f;
        Ray[] rays = new Ray[4] { 
            new Ray(transform.position + (transform.forward * 0.3f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.3f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.3f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.3f) + (Vector3.up * 0.01f), Vector3.down),
        };

        // 그 후 Raycast를 통해 바닥에 닿으면 true를 하나도 닿지 않으면 false를 반환한다.
        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], rayDistance, groundMask))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        float rayDistance = 0.2f;
        Ray[] rays = new Ray[4] {
            new Ray(transform.position + (transform.forward * 0.3f) + (Vector3.up * 0.01f), Vector3.down * rayDistance),
            new Ray(transform.position + (-transform.forward * 0.3f) + (Vector3.up * 0.01f), Vector3.down * rayDistance),
            new Ray(transform.position + (transform.right * 0.3f) + (Vector3.up * 0.01f), Vector3.down * rayDistance),
            new Ray(transform.position + (-transform.right * 0.3f) + (Vector3.up * 0.01f), Vector3.down * rayDistance),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i]);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        animator.SetFloat("InputDir", inputDir.magnitude);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // 스태미너 확인 필요
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

    public void OnJump(InputAction.CallbackContext context)
    {
        // 스태미너 확인 필요
        if(context.phase == InputActionPhase.Started && !isJump && IsGround())
        {
            Jump();
        }
    }

    private void Attack()
    {
        isAttack = true;

        Interaction interaction = GetComponent<Interaction>();

        if (interaction.rock != null)
        {
            // 곡괭이가 있고 Rock을 가리키고 있을 땐 RockAttack
            animator.SetTrigger("RockAttack");
        }
        else if(interaction.tree != null)
        {
            // 도끼가 있고 Tree를 가리키고 있을 땐 TreeAttack
            animator.SetTrigger("TreeAttack");
        }
        else
        {
            // 무기가 없을 땐 기본 Attack
            animator.SetTrigger("Attack");

            // 무기가 있을 땐 EquipAttack
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // 스태미너 확인 필요
        if(context.phase == InputActionPhase.Started && !isAttack && !isJump)
        {
            Attack();
        }
    }

    public void EndJump()
    {
        isJump = false;
    }

    public void EndAttack()
    {
        isAttack = false;
        Debug.Log("End Attack");
    }

    private void OnAnimatorMove()
    {
        
    }
}
