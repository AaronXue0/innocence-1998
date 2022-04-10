using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Bag", menuName = "Innocene/Bag", order = 5)]
    public class Bag : ScriptableObject
    {
        [SerializeField] List<GameItem> inStorageItem;

        public List<int> GetItemsID()
        {
            List<int> items = new List<int>();
            foreach (GameItem item in inStorageItem)
            {
                items.Add(item.id);
            }
            return items;
        }

        public void Reset()
        {
            inStorageItem = new List<GameItem>();
        }

        public void StoreItem(GameItem item)
        {
            inStorageItem.Add(item);
        }

        public void RemoveItem(GameItem item) => inStorageItem.Remove(item);

        public void RemoveItem(int id)
        {
            GameItem item = inStorageItem.Find(x => x.id == id);
            inStorageItem.Remove(item);
            BagManager.Instance.DeleteItem(id);
        }

        public bool CheckInStorage(int id)
        {
            GameItem item = inStorageItem.Find(x => x.id == id);
            if (item == null)
                return false;
            return true;
        }
    }
}