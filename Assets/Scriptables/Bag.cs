using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Item", menuName = "Innocene/Bag", order = 0)]
    public class Bag
    {
        [SerializeField] List<GameItem> inStorageItem;

        public void StoreItem(GameItem item)
        {
            inStorageItem.Add(item);
        }

        public void RemoveItem(GameItem item) => inStorageItem.Remove(item);

        public void RemoveItem(int id)
        {
            GameItem item = inStorageItem.Find(x => x.id == id);
            inStorageItem.Remove(item);
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