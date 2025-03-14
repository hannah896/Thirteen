using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData ItemData;

    public enum resourceType 
    {
        Mine, //채광
        Lumber, //벌목
        Gathering //채집
    }

    // TODO: 인벤토리 만들어졌을때 넣는 기능 필요.
}