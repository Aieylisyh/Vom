using UnityEngine;
using DG.Tweening;
using com;
using System.Collections.Generic;
using UnityEngine.UI;

namespace game
{
    public class WindowInventoryBehaviour : MonoBehaviour
    {
        public List<SlotBehaviour> slots;
        public bool isInventoryOrShop;
        public bool hideEmpty = false;
        public ResizeRectTransform resizer;
        public ShopService.ShopCategory shopCategory;

        private static List<WindowInventoryBehaviour> _currentDisplayingInstances = new List<WindowInventoryBehaviour>();

        public static void RefreshCurrentDisplayingInstances()
        {
            foreach (var i in _currentDisplayingInstances)
            {
                i.Refresh();
            }
        }

        public void ToggleCurrentDisplayingInstance(bool isAdd)
        {
            if (isAdd)
            {
                if (!_currentDisplayingInstances.Contains(this))
                {
                    _currentDisplayingInstances.Add(this);
                }
            }
            else
            {
                if (_currentDisplayingInstances.Contains(this))
                {
                    _currentDisplayingInstances.Remove(this);
                }
            }
        }

        public void Clear()
        {
            Setup(new List<Item>());
        }

        private void CheckHideLines(int count)
        {
            if (!hideEmpty)
                return;

            int slotCount = 0;
            foreach (var s in slots)
            {
                s.gameObject.SetActive(slotCount < count);
                slotCount++;
            }
        }

        public void Refresh()
        {
            //Debug.Log("Refresh!");
            if (isInventoryOrShop)
            {
                //inv
                UxService.instance.SortItems();
                Setup(UxService.instance.GetInventoryItems());
            }
            else
            {
                //shop
                foreach (var s in slots)
                {
                    s.shopCategory = shopCategory;
                }

                Setup(ShopService.instance.Fetch(true, shopCategory));
            }
            resizer?.ResizeLater();
        }

        public void Setup(List<CommoditySlotData> commodities)
        {
            CheckHideLines(commodities.Count);
            int i = -1;
            foreach (var s in slots)
            {
                i++;
                s.SetEmpty();
                if (commodities.Count > i)
                {
                    var c = commodities[i];
                    if (c != null)
                    {
                        s.Setup(c);
                    }
                }
            }
        }

        public void Setup(List<Item> items)
        {
            CheckHideLines(items.Count);
            if (items == null)
            {
                foreach (var s in slots)
                {
                    s.SetEmpty();
                }
            }
            int i = -1;
            foreach (var s in slots)
            {
                i++;
                if (items.Count > i)
                {
                    s.Setup(items[i]);
                }
                else
                {
                    s.SetEmpty();
                }
            }
        }
    }
}
