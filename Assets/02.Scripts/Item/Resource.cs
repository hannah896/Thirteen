using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Resource : MonoBehaviour
{
    public int maxAmount; //�ִ�� Ķ���ִ� ��
    public int currentChances; //���� Ķ �� �ִ� Ƚ��
    public float second; //��Ÿ�� ���� �ð�
    public GameObject resource;

    public Collider Collider;

    private void OnValidate()
    {
        Collider = GetComponentInChildren<Collider>();
    }

    //�ڿ��� �����ϴ� ���� ��ȯ
    public void MakingResource()
    {
        if (currentChances <= 0) return;
        currentChances--;
        Instantiate(resource, transform.position +  new Vector3(
            0.3f * Collider.bounds.size.x,
            0.1f * Collider.bounds.size.y,
            0
            ), Quaternion.identity);
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
