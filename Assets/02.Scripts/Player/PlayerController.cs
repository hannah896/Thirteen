using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    private float runStemina = 0.1f;                // 달리기 스태미나

    private Animator animator;

    private bool isJump;                        // 점프 상태 확인
    [SerializeField] private float jumpForce;   // 점프 파워
    private float jumpStemina = 10;                  // 점프 스태미나

    [SerializeField] private LayerMask groundMask;  // 땅을 표시하는 Layer

    [SerializeField]private bool isAttack;                      // 공격 상태 확인
    private float attackStemina = 10;                // 공격 스태미나
    [SerializeField] Transform attackTr;
    [SerializeField] float attackRange = 2f;
    [SerializeField] private LayerMask enemyMask;

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
        animator.SetBool("IsRun", isRun);
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
        CharacterManager.Instance.Player.condition.UseStamina(jumpStemina);
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

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        animator.SetFloat("InputDir", inputDir.magnitude);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }


    private Coroutine RunCo = null;
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            RunCo = StartCoroutine(Run());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            StopCoroutine(RunCo);
            isRun = false;
        }
    }

    private IEnumerator Run()
    {
        isRun = true;
        while (CharacterManager.Instance.Player.condition.UseStamina(runStemina))
        {
            yield return null;
        }
        isRun = false;
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

        Resource resource = CharacterManager.Instance.Player.resource;

        // 캐릭터가 자원을 가리키고 있다면 자원을 캐자
        if(resource != null)
        {
            HitResource(resource);
        }
        else
        {
            // 스태미너가 적절하게 남아있는가?
            if(CharacterManager.Instance.Player.condition.UseStamina(attackStemina))
            {
                // 무기가 없을 땐 기본 Attack
                //if(CharacterManager.Instance.Player.equip == null)
                animator.SetTrigger("Attack");

                //if(CharacterManager.Instance.Player.equip != null)
                // 무기가 있을 땐 EquipAttack
                //animator.SetTrigger("EquipAttack");
            }
        }
    }

    // 자원을 캐는 함수
    private void HitResource(Resource resource)
    {
        switch (resource.resourceType)
        {
            case ResourceType.Mine:
                // 곡괭이가 있고 Rock을 가리키고 있을 땐 RockAttack
                animator.SetTrigger("RockAttack");
                break;
            case ResourceType.Lumber:
                // 도끼가 있고 Tree를 가리키고 있을 땐 TreeAttack
                animator.SetTrigger("TreeAttack");
                break;
            case ResourceType.Gathering:
                break;
            default:
                break;
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

    // 캐는 애니메이션에서 적절한 프레임에 발생시키는 이벤트
    // 리소스가 있다면 자원 아이템을 생성시킨다.
    public void OnResourceHit()
    {
        if (CharacterManager.Instance.Player.resource == null) return;

        CharacterManager.Instance.Player.resource.MakingResource();
    }

    public void OnEnemyHit()
    {
        Collider[] colliders = Physics.OverlapSphere(attackTr.position, attackRange, enemyMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IDamageable enemy))
            {
                Debug.Log("몬스터 Hit");
                enemy.TakeDamage(CharacterManager.Instance.Player.condition.AttackDamage);
            }
        }
    }

    private void OnAnimatorMove()
    {

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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTr.position, attackRange);
    }
}
