using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPlayer : MonoBehaviour
{
    public Equipment curEquip;      // 현재 장착 중인 장비
    public Transform equipParent;   // 장비가 장착될 부모 Transform

    public void Equip(ItemData data)
    {
        UnEquip();

        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equipment>();

        if (curEquip != null)
        {
            curEquip.gameObject.transform.localPosition = curEquip.localPosition;
            curEquip.gameObject.transform.localRotation = Quaternion.Euler(curEquip.localRotation);
            curEquip.gameObject.transform.localScale = curEquip.localScale;
        }

        // 콜라이더가 활성화 되어있다면 꺼주기
        if(curEquip.TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }

        if(curEquip.TryGetComponent(out Rigidbody rigid))
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
        }

        CharacterManager.Instance.Player.condition.SetAttackDamage((int)curEquip.GetComponent<Equipment>().AttackPower);
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
