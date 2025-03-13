using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int maxAmount; //최대로 캘수있는 양
    public int currentChances; //현재 캘 수 있는 횟수
    public float second; //쿨타임 도는 시간
    public GameObject resource;

    //자원을 생성하는 것을 반환
    public void MakingResource()
    {
        if (currentChances < 0) return;
        currentChances--;
        Instantiate(resource, transform.position,Quaternion.identity);
        Debug.Log("캐다");
        if (currentChances == 0)
        {
            StartCoroutine(ResetMaxinum());
        }
    }

    //자원 초기화시간
    private IEnumerator ResetMaxinum()
    {
        Debug.Log("초기화중");
        yield return new WaitForSeconds(second);
        currentChances = maxAmount;
        yield break;
    }
}
