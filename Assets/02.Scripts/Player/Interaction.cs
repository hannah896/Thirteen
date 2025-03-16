using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    [SerializeField] float resourceRayDistance = 1f;    // 자원을 감지하는 Ray의 길이
    [SerializeField] float itemRayDistance = 5f;        // 아이템을 감지하는 Ray의 길이
    [SerializeField] LayerMask resourceMask;        // 캘 수 있는 자원의 마스크
    [SerializeField] LayerMask interactableMask;    // 획득 할 수 있는 아이템의 마스크

    public GameObject itemGO;
    public ItemData itemData;                              // 감지한 아이템

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
            RayCastResource();
            RayCastItem();
        }
    }

    // 자원을 캐스팅 하는지 확인
    private void RayCastResource()
    {
        // 캐릭터의 발 쪽에서 ray를 쏘기위해 transform을 가져온다.
        Transform CharacterTr = transform.GetChild(0);
        // 캐릭터의 발쪽에서 ray 위치 설정
        Vector3 rayOrigin = CharacterTr.position + Vector3.up * 0.5f;
        Ray ray = new Ray(rayOrigin, new Vector3(CharacterTr.forward.x, 0, CharacterTr.forward.z));

        Debug.DrawRay(ray.origin, CharacterTr.forward * resourceRayDistance, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, resourceRayDistance, resourceMask))
        {
            if (hit.transform.TryGetComponent(out Resource resource))
            {
                CharacterManager.Instance.Player.resource = resource;
            }
        }
        else
        {
            CharacterManager.Instance.Player.resource = null;
        }
    }

    // 아이템을 캐스팅 하는지 확인
    private void RayCastItem()
    {
        // 화면의 중앙에서 ray를 발사하여 감지
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * itemRayDistance, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, itemRayDistance, interactableMask))
        {
            if (hit.transform.TryGetComponent(out ItemObject item))
            {
                itemGO = hit.collider.gameObject;
                itemData = item.ItemData;
            }
        }
        else
        {
            itemGO = null;
            itemData = null;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && itemData != null)
        {
            // 인벤토리에 저장

            CharacterManager.Instance.Player.itemData = itemData;

            CharacterManager.Instance.Player.AddItem();
            Destroy(itemGO);
            itemGO = null;
            itemData = null;
        }
    }
}
