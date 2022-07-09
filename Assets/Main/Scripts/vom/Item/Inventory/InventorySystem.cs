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
            AddItemFeedback(data);
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

        public void AddItemFeedback(ItemData data)
        {
            //game.FloatingTextPanelBehaviour.instance.Create("<sprite name=Diamond> +1", PlayerBehaviour.instance.transform);
            if (data.id == "Gold")
            {
                // game.FloatingTextPanelBehaviour.instance.Create("<sprite name=Gold><size=75%><color=#FFFFAA>+" + data.n + "</color></size>",   0.5f + Random.value * 0.12f, 0.966f);
                game.FloatingTextPanelBehaviour.instance.Create("<sprite name=Gold><size=80%><color=#FFFFAA>+" + data.n + "</color></size>", PlayerBehaviour.instance.transform);
            }
            else if (data.id == "Soul")
            {
                game.FloatingTextPanelBehaviour.instance.Create("<sprite name=Diamond><size=80%><color=#AAFFFF>+" + data.n + "</color></size>", PlayerBehaviour.instance.transform);
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