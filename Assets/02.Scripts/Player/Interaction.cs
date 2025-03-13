using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

//public interface IInteraction
//{
//    public void ShowDisplay();
//}

public class Interaction : MonoBehaviour
{
    private float interactionDelay = 0.05f;  // 감지 딜레이 (프레임 별 감지하는 걸 방지하기 위해)
    private float lastInteractionTime;      // 마지막으로 감지한 시간

    [SerializeField] float rayDistance = 5f;
    [SerializeField] LayerMask resourceMask;        // 캘 수 있는 자원의 마스크
    [SerializeField] LayerMask interactableMask;    // 획득 할 수 있는 아이템의 마스크

    public GameObject rock;
    public GameObject tree;

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 마지막으로 감지한 시간이 딜레이 시간보다 지났다면
        if(Time.time - lastInteractionTime > interactionDelay)
        {
            // 마지막 감지한 시간을 현재로 설정
            lastInteractionTime = Time.time;

            Vector3 rayOrigin = transform.GetChild(0).position + Vector3.up * 0.5f;
            Ray resourceRay = new Ray(rayOrigin, new Vector3(transform.GetChild(0).forward.x, 0, transform.GetChild(0).forward.z));

            Debug.DrawRay(resourceRay.origin, transform.GetChild(0).forward * rayDistance, Color.red);
            if (Physics.Raycast(resourceRay, out RaycastHit hit, rayDistance, resourceMask))
            {
                if(hit.transform.TryGetComponent(out Resource resource))
                {
                    CharacterManager.Instance.Player.resource = resource;
                }

                if(hit.transform.name == "Rock")
                {
                    tree = null;
                    rock = hit.transform.gameObject;
                }
                else if(hit.transform.name == "Tree")
                {
                    rock = null;
                    tree = hit.transform.gameObject;
                }
            }
            else
            {
                rock = null;
                tree = null;
                CharacterManager.Instance.Player.resource = null;
            }

            // 주울 수 있는 아이템 감지
            Ray interactableRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Debug.DrawRay(interactableRay.origin, interactableRay.direction * rayDistance, Color.red);
            if (Physics.Raycast(interactableRay, out RaycastHit interactableHit, rayDistance, interactableMask))
            {
                if (hit.transform.TryGetComponent(out ItemData itemData))
                {
                    CharacterManager.Instance.Player.itemData = itemData;
                }
            }
            else
            {
                CharacterManager.Instance.Player.itemData = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        ray.direction *= rayDistance;
        Gizmos.DrawRay(ray);
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && CharacterManager.Instance.Player.itemData != null)
        {
            // 인벤토리에 저장
            //item.OnInteraction();
            //item = null;
            //Destroy(item.gameObject);
        }
    }
}
