using UnityEngine;
using System;
using com;
using System.Collections.Generic;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ItemsPopup : PopupBehaviour
    {
        public Text title;
        public Text content;
        public WindowInventoryBehaviour inv;
        public Text btnTxt;
        private ItemsPopupData _data;
        public GameObject btnOk;
        public ResizeRectTransform resizer;
        private Action _okCb;

        public struct ItemsPopupData
        {
            public List<Item> items;
            public string title;
            public string content;
            public bool clickBgClose;
            public bool hasBtnOk;
            public Action OkCb;
            public string btnOkString;
        }

        public void Setup(ItemsPopupData data)
        {
            _data = data;

            SortItems();
            inv.Setup(_data.items);
            title.text = _data.title;
            content.text = _data.content;

            if (string.IsNullOrEmpty(_data.btnOkString))
                btnTxt.text = LocalizationService.instance.GetLocalizedText("OK");
            else
                btnTxt.text = _data.btnOkString;

            btnOk.SetActive(_data.hasBtnOk);
            resizer.ResizeLater();
            _okCb = _data.OkCb;
        }

        void SortItems()
        {
            var newList = new List<Item>();
            var cfg = ConfigService.instance.itemConfig;

            foreach (var c in cfg.list)
            {
                foreach (var item in _data.items)
                {
                    if (item != null && c != null && c.id == item.id)
                    {
                        if (item.n > 0)
                        {
                            newList.Add(new Item(item.n, item.id));
                        }
                        break;
                    }
                }
            }

            _data.items = newList;
        }

        public override void OnClickBtnClose()
        {
            if (!_data.clickBgClose)
            {
                return;
            }

            Hide();
            Sound();
        }

        public void OnClickOkBtn()
        {
            _okCb?.Invoke();
            Hide();
            Sound();
        }
    }
}