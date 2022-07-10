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

        public int GoldCount { get { return GetItemCount("Gold"); } }
        public int SoulCount { get { return GetItemCount("Soul"); } }

        public int GetItemCount(string id)
        {
            foreach (var item in items)
            {
                if (item.id == id)
                    return item.n;
            }

            return 0;
        }

        public void AddItem(string id, int num = 1)
        {
            AddItem(new ItemData(num, id));
        }

        public void AddItemFeedback(ItemData data)
        {
            if (data.id == "Gold")
            {
                MainHudBehaviour.instance.SyncGold();
            }
            else if (data.id == "Soul")
            {
                MainHudBehaviour.instance.SyncSoul();
            }
            else if (data.id == "Exp")
            {
                MainHudBehaviour.instance.SyncExp();
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