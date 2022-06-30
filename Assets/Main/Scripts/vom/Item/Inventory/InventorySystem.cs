using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem instance { get; private set; }

        public List<ItemData> items { get; private set; }

        private void Awake()
        {
            instance = this;
            items = new List<ItemData>();
        }

        public void AddItem(ItemData data)
        {
            foreach (var item in items)
            {
                if (item.id == data.id)
                {
                    item.n += data.n;
                    return;
                }
            }

            items.Add(data);
        }

        public void AddItem(string id, int num = 1)
        {
            AddItem(new ItemData(num, id));
        }

        public void SortItems()
        {

        }
    }
}