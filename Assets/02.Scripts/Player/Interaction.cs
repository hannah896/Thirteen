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
    [SerializeField] LayerMask InteractionMask;

    public GameObject rock;
    public GameObject tree;

    //public IInteraction interactionObj;

    //ItemData itemData;

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

            //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Vector3 rayOrigin = transform.GetChild(0).position + Vector3.up * 0.5f;
            Ray ray = new Ray(rayOrigin, new Vector3(transform.GetChild(0).forward.x, 0, transform.GetChild(0).forward.z));

            Debug.DrawRay(ray.origin, transform.GetChild(0).forward * rayDistance, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, InteractionMask))
            {
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
                //if(hit.transform.TryGetComponent<IInteraction>(out IInteraction interaction))
                //{
                //    interactionObj = interaction;
                //    if(interactionObj is ItemData)
                //    {
                //        ItemData data = interactionObj as ItemData;
                //        itemData = data;
                //    }
                //}
            }
            else
            {
                rock = null;
                tree = null;
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
        //if(context.phase == InputActionPhase.Started && itemData != null)
        //{
        //    // 인벤토리에 저장
        //    itemData = null;
        //    Destroy(itemData.);
        //}
    }
}
