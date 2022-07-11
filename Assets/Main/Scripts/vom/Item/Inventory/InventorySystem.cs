using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem instance { get; private set; }

        List<ItemData> _items = new List<ItemData>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            InitData();
        }

        void InitData()
        {
            _items = new List<ItemData>();
            _items.Add(new ItemData(33, "Exp"));

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

            foreach (var item in _items)
            {
                if (item.id == data.id)
                {
                    item.n += data.n;
                    break;
                }
            }

            _items.Add(data);
            AddItemFeedback(data);
        }

        public int GoldCount { get { return GetItemCount("Gold"); } }
        public int SoulCount { get { return GetItemCount("Soul"); } }
        public int ExpCount { get { return GetItemCount("Exp"); } }

        public int GetItemCount(string id)
        {
            foreach (var item in _items)
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
            _items.Sort(InventoryService.CompareItem);
        }
    }
}