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
                game.FloatingTextPanelBehaviour.instance.Create("<color=#FFFF00>+1</color>", 0.5f, 0.9f);
            }
            else if (data.id == "Soul")
            {
                game.FloatingTextPanelBehaviour.instance.Create("<color=#00FF99>+1</color>", 0.7f, 0.9f);
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