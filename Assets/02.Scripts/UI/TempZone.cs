using System.Collections.Generic;
using UnityEngine;

public class TempZone : MonoBehaviour
{
    [SerializeField]
    private BodyTemp bodyTemp;

    public enum ZoneType
    {
        Hot,
        Cold
    }    

    [SerializeField]
    private ZoneType zoneType;
 

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
}
