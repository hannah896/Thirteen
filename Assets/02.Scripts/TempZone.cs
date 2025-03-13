using System.Collections.Generic;
using UnityEngine;

public class TempZone : MonoBehaviour
{
    [SerializeField]
    private BodyTemp bodyTemp;

    //public int damage;
    //public float damageRate;

    //List<IDamageable> things = new List<IDamageable>();

    public enum ZoneType
    {
        Hot,
        Cold
    }    

    [SerializeField]
    private ZoneType zoneType;

    //private void Start()
    //{
    //    InvokeRepeating("DealDamage", 0, damageRate);
    //}

    //void DealDamage()
    //{
    //    for (int i = 0; i < things.Count; i++)
    //    {
    //        things[i].TakeDamage(damage);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoneType == ZoneType.Hot)
            {
                bodyTemp.Hot();
            }
            else
            {
                bodyTemp.Cold();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bodyTemp.ExitTempZone();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.TryGetComponent(out IDamageable damageable))
    //    {
    //        things.Add(damageable);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.TryGetComponent(out IDamageable damageable))
    //    {
    //        things.Remove(damageable);
    //    }
    //}
}
