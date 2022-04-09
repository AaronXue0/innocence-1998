using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Innocence;

[CreateAssetMenu(fileName = "ItemList", menuName = "Game/Item/Item Info List")]
public class ItemInfoList : ScriptableObject
{
    public List<ItemInfo> items;
    [HideInInspector]
    public List<int> GetInBagItemsID
    {
        get
        {
            Bag bag = GameManager.instance.GetBag;
            List<ItemInfo> inBagItems = new List<ItemInfo>();
            return bag.GetItemsID();
        }
    }

    public ItemInfo GetItemWithID(int id)
    {
        return items.Find(x => x.id == id);
    }
}