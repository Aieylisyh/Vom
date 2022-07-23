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
            slotCount = 15;
        }

        void InitData()
        {
            items = new List<ItemData>();
            AddItem(new ItemData(33, "Exp"), false);
            AddItem(new ItemData(33, "Apple"), false);
            AddItem(new ItemData(33, "Wood"), false);
            AddItem(new ItemData(33, "Fish"), false);
            AddItem(new ItemData(33, "Shit"), false);

            MainHudBehaviour.instance.SyncGold();
            MainHudBehaviour.instance.SyncSoul();
            DailyPerkSystem.instance.SyncExp();
        }

        //AddItem(new ItemData(num, id));
        //this is the final step to add an item, check feedbacks here
        public void AddItem(ItemData data, bool feedback = true)
        {
            if (data.n == 0)
                return;

            var p = ItemService.GetPrototype(data.id);
            if (feedback)
                AddItemFeedback(data);

            foreach (var item in items)
            {
                if (item.id == data.id)
                {

                    var canAddN = p.stack - item.n;
                    if (canAddN >= data.n)
                    {
                        item.n += data.n;
                        return;
                    }
                    else
                    {
                        item.n = p.stack;
                        data.n = data.n - canAddN;
                        break;
                    }
                }
            }

            while (data.n > p.stack)
            {
                items.Add(new ItemData(p.stack, data.id));
                data.n = data.n - p.stack;
            }

            items.Add(data);
        }

        public bool TryRemoveItem(bool validateRemove = true)
        {
            var enough = false;
            return enough;
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
            Debug.Log(data.id + " " + data.n);
        }
    }
}