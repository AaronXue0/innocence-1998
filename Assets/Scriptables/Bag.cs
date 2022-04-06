using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Bag", menuName = "Innocene/Bag", order = 5)]
    public class Bag : ScriptableObject
    {
        [SerializeField] List<GameItem> inStorageItem;

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