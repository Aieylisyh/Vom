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

        //this is the final step to add an item, check feedbacks here
        public void AddItem(ItemData data)
        {
            foreach (var item in items)
            {
                if (item.id == data.id)
                {
                    item.n += data.n;
                    break;
                }
            }

            items.Add(data);
            AddItemFeedback(data);
        }

        public void AddItem(string id, int num = 1)
        {
            AddItem(new ItemData(num, id));
        }

        public void AddItemFeedback(ItemData data)
        {
            if (data.id == "Gold")
            {
                MainHudBehaviour.instance.SyncGold(false);
            }
            else if (data.id == "Soul")
            {
                MainHudBehaviour.instance.SyncSoul(false);
            }
            else if (data.id == "Exp")
            {
                MainHudBehaviour.instance.SyncExp(false);
            }
            else
            {
                ToastSystem.instance.Add(data);
            }
        }

        public void SortItems()
        {
            items.Sort(InventoryService.CompareItem);
        }
    }
}