using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem instance { get; private set; }

        public List<ItemData> items { get; private set; }
        public int slotCount { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            items = new List<ItemData>();
            InitData();
            slotCount = 10;
        }

        void InitData()
        {
            items = new List<ItemData>();
            items.Add(new ItemData(33, "Exp"));

            MainHudBehaviour.instance.SyncGold();
            MainHudBehaviour.instance.SyncSoul();
            DailyPerkSystem.instance.SyncExp();
        }

        //AddItem(new ItemData(num, id));
        //this is the final step to add an item, check feedbacks here
        public void AddItem(ItemData data)
        {
            if (data.n == 0)
                return;

            foreach (var item in items)
            {
                if (item.id == data.id)
                {
                    item.n += data.n;
                    return;
                }
            }

            items.Add(data);
            AddItemFeedback(data);
        }

        public int GoldCount { get { return GetItemCount("Gold"); } }
        public int SoulCount { get { return GetItemCount("Soul"); } }
        public int ExpCount { get { return GetItemCount("Exp"); } }

        public int GetItemCount(string id)
        {
            foreach (var item in items)
            {
                if (item.id == id)
                    return item.n;
            }

            return 0;
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
                DailyPerkSystem.instance.SyncExp();
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

        public void OnClickInvItem(ItemData data)
        {

        }
    }
}