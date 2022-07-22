using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using System.Collections.Generic;

namespace vom
{
    public class InventoryBehaviour : WindowBehaviour
    {
        public game.ResizeRectTransform resizer;

        List<InvSlotBehaviour> _slots;
        public InvSlotBehaviour slotPrefab;

        protected override void Awake()
        {
            base.Awake();
            _slots = new List<InvSlotBehaviour>();
        }

        void RemoveUneceSlots(int slotCount)
        {
            for (int i = _slots.Count; i > 0; i--)
            {
                if (i > slotCount)
                {
                    //remove this slot
                    Destroy(_slots[i - 1].gameObject);
                    _slots.RemoveAt(i - 1);
                }
            }
        }

        void AddNeceSlots(int slotCount)
        {
            for (int i = 0; i < slotCount; i++)
            {
                if (i > _slots.Count - 1)
                {
                    //remove this slot
                    var slot = Instantiate(slotPrefab, slotPrefab.transform.parent);
                    slot.gameObject.SetActive(true);
                    _slots.Add(slot);
                }
            }
        }

        public override void Setup()
        {
            base.Setup();
            Sync();
        }

        public void Sync()
        {
            //Debug.Log("Sync");
            InventorySystem.instance.SortItems();
            var slotCount = InventorySystem.instance.slotCount;
            RemoveUneceSlots(slotCount);
            AddNeceSlots(slotCount);
            //Debug.Log(_slots.Count);
            var items = InventorySystem.instance.items;
            int itemIndex = -1;
            for (int i = 0; i < slotCount; i++)
            {
                itemIndex++;
                if (itemIndex > items.Count - 1)
                {
                    _slots[i].SetEmpty();
                    continue;
                }

                var item = items[itemIndex];
                // Debug.Log(item.id + " " + item.n);
                var p = ItemService.GetPrototype(item.id);
                if (p.invHide)
                {
                    i -= 1;
                    continue;
                }
                else
                {
                    _slots[i].Sync(item);
                }
            }

            resizer.ResizeLater();
        }
    }
}