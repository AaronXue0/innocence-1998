using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "Game/Item/Item Info List")]
public class ItemInfoList : ScriptableObject
{
    public List<ItemInfo> items;
}