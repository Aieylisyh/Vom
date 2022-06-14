using UnityEngine;
using UnityEngine.UI;
using com;
using System.Collections.Generic;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class AmountSelectPopup : PopupBehaviour
    {
        public Slider slider;
        public Text amountText;
        public Text priceText;
        private int _min = 1;
        private int _max = 0;
        private int _crtAmount = 0;
        private CommodityPopup _commodityPopup;
        private ItemPopup _itemPopup;
        private CommoditySlotData _commodity;
        private Item _item;
        private ItemPrototype.Usage usage;

        public void Setup(CommoditySlotData commodity, CommodityPopup p, int max)
        {
            Reset();
            _commodityPopup = p;
            _commodity = commodity;
            _max = max;
            usage = ItemPrototype.Usage.Transaction_limited;
            //var outputItemProto = ItemService.instance.GetPrototype(commodityProto.itemOutPut.id);
            //priceText.text = " Drag the round button to change";
            OnSliderChange(0);
        }

        public void Setup(Item i, ItemPopup p)
        {
            Reset();
            _itemPopup = p;
            _item = i;
            _max = UxService.instance.GetItemAmount(i.id);
            var proto = ItemService.instance.GetPrototype(i.id);
            usage = proto.usage;
            OnSliderChange(0);
        }

        public void Setup(Item i, ItemPopup p, int max)
        {
            Reset();
            _itemPopup = p;
            _item = i;
            _max = max;
            var proto = ItemService.instance.GetPrototype(i.id);
            usage = proto.usage;
            OnSliderChange(0);
        }

        public void Reset()
        {
            _commodityPopup = null;
            _itemPopup = null;
            _commodity = null;
            _item = null;
            _crtAmount = 1;
            slider.value = 0;
            amountText.text = _crtAmount + " ";
            priceText.text = "";
        }

        public void OnSliderChange(float v)
        {
            int amount = 0;
            if (v > 0.99f)
            {
                amount = _max;
            }
            else
            {
                amount = Mathf.FloorToInt((float)_max * v + _min);
            }

            slider.value = v;
            _crtAmount = amount;
            amountText.text = _crtAmount + " ";
            if (_commodity != null)
            {
                SetAmountString(GetItemListWithMultiplier(_commodity.commodity.itemValue, amount));
            }
            else if (_item != null)
            {
                var itemProto = ItemService.instance.GetPrototype(_item.id);
                SetAmountString(GetItemListWithMultiplier(itemProto.itemValue, amount));
            }
        }

        public void OnClickOkBtn()
        {
            //Debug.Log("AmountSelectPopup OnClickOkBtn "+ _crtAmount);
            _commodityPopup?.OnAmountSelected(_crtAmount);
            _itemPopup?.OnAmountSelected(_crtAmount);

            Hide();
            Sound();
        }

        public List<Item> GetItemListWithMultiplier(List<Item> items, int m)
        {
            List<Item> list = new List<Item>();
            foreach (var i in items)
            {
                list.Add(new Item(m * i.n, i.id));
            }
            return list;

        }

        public void SetAmountString(List<Item> prices)
        {
            switch (usage)
            {
                case ItemPrototype.Usage.Transaction_unlimited:
                case ItemPrototype.Usage.Transaction_limited:
                    priceText.text = LocalizationService.instance.GetLocalizedText("Cost") + ":" + TextFormat.GetItemText(prices, true);
                    break;

                case ItemPrototype.Usage.Sell:
                    priceText.text = LocalizationService.instance.GetLocalizedText("WillGet") + ":" + TextFormat.GetItemText(prices, true);
                    break;

                case ItemPrototype.Usage.Open:
                    priceText.text = LocalizationService.instance.GetLocalizedText("Open") + ":" + _crtAmount + " " + TextFormat.GetRichTextTag(_item.id);
                    break;

                case ItemPrototype.Usage.Craft:
                    priceText.text = LocalizationService.instance.GetLocalizedText("Need") + ":" + TextFormat.GetItemText(prices, true);
                    break;

                case ItemPrototype.Usage.Consume:
                    priceText.text = LocalizationService.instance.GetLocalizedText("Use") + ":" + _crtAmount + " " + TextFormat.GetRichTextTag(_item.id);
                    break;

                case ItemPrototype.Usage.CheckOut:
                case ItemPrototype.Usage.None:
                    priceText.text = "";
                    break;
            }
        }
    }
}
