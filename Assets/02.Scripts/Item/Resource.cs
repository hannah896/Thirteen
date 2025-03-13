using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int maxAmount; //�ִ�� Ķ���ִ� ��
    public int currentChances; //���� Ķ �� �ִ� Ƚ��
    public float second; //��Ÿ�� ���� �ð�
    public GameObject resource;

    //�ڿ��� �����ϴ� ���� ��ȯ
    public void MakingResource()
    {
        if (currentChances < 0) return;
        currentChances--;
        Instantiate(resource, transform.position,Quaternion.identity);
        Debug.Log("ĳ��");
        if (currentChances == 0)
        {
            StartCoroutine(ResetMaxinum());
        }
    }

    //�ڿ� �ʱ�ȭ�ð�
    private IEnumerator ResetMaxinum()
    {
        Debug.Log("�ʱ�ȭ��");
        yield return new WaitForSeconds(second);
        currentChances = maxAmount;
        yield break;
    }
}
