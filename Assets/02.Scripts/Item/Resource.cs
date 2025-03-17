using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ResourceType
{
    Mine, //채광
    Lumber, //벌목
    Gathering, //채집
    Hunting //사냥
}

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;

    public GameObject[] dropItems;
    public Collider collider;

    public int maxAmount; //최대로 캘수있는 양
    public int currentChances; //현재 캘 수 있는 횟수
    public float second; //쿨타임 도는 시간

    private void OnValidate()
    {
        collider = GetComponentInChildren<Collider>();
    }

    //자원을 생성하는 것을 반환
    public void MakingResource()
    {
        if (currentChances <= 0) return;
        currentChances--;


        Instantiate(dropItems[Random.Range(0, dropItems.Length)], 
            collider.transform.position + new Vector3
            (
                0,
                0.1f * collider.bounds.size.y,
                0
            ), 
            Quaternion.identity);

        Debug.Log("캐다");
        if (currentChances == 0)
        {
            StartCoroutine(ResetMaximum());
        }
    }

    //자원 초기화시간
    private IEnumerator ResetMaximum()
    {
        Debug.Log("초기화중");
        yield return new WaitForSeconds(second);
        currentChances = maxAmount; 
        yield break;
    }
}
