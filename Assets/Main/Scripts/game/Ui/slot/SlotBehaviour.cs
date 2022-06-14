using UnityEngine;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class SlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public Text label;
        private Item _item;

        public GameObject promoBanner;
        public Text promoText;

        public float imageSize = 100;
        private CommoditySlotData _commodity;
        public bool enableUseItem;
        public bool isShopSlot;
        public ShopService.ShopCategory shopCategory;
        public int indexOfList = -1;
        public bool noUseScientificNote;

        void CheckIndexOfList()
        {
            if (indexOfList < 0)
            {
                indexOfList = this.transform.GetSiblingIndex();
            }
        }

        public void Setup(Item param)
        {
            CheckIndexOfList();
            _item = param;
            _commodity = null;
            if (param == null)
            {
                SetEmpty();
                return;
            }

            ItemPrototype itemProto = ItemService.instance.GetPrototype(_item.id);
            //Debug.Log(_item.id);
            SetView(itemProto.sp, _item.n);
        }

        public void Setup(CommoditySlotData param)
        {
            CheckIndexOfList();
            _commodity = param;
            _item = null;
            if (param == null)
            {
                SetEmpty();
                return;
            }

            ItemPrototype commodityProto = param.commodity;
            ItemPrototype itemProto = ItemService.instance.GetPrototype(commodityProto.itemOutPut.id);
            SetView(itemProto.sp, commodityProto.itemOutPut.n);
        }

        private void SetView(Sprite sp, int amount)
        {
            icon.enabled = true;
            icon.sprite = sp;

            if (amount <= 1)
            {
                label.text = "";
                return;
            }

            label.text = (noUseScientificNote) ? amount.ToString() : TextFormat.GetBigNumberScientific(amount);
        }

        public void SetEmpty()
        {
            //Debug.Log("SetEmpty");
            _item = null;
            _commodity = null;
            icon.enabled = false;
            label.text = "";
        }

        public void Awake()
        {
            icon.rectTransform.sizeDelta = new Vector2(imageSize, imageSize);
        }

        public void OnClick()
        {
            //Debug.Log("slot OnClickOkBtn");
            if (_item != null)
            {
                //Debug.Log("is an item");
                // ItemPrototype proto = ItemService.instance.GetPrototype(_item.id);
                var data = new ItemPopup.ItemPopupData();
                data.enableUse = enableUseItem;
                data.item = new Item(1, _item.id);
                WindowService.instance.ShowItemPopup(data);
                SoundService.instance.Play("tap");
                return;
            }

            if (_commodity != null)
            {
                //Debug.Log("is a commodity");
                if (isShopSlot)
                {
                    var data = new CommodityPopup.CommodityPopupData();
                    data.commodity = _commodity;
                    data.shopCategory = shopCategory;
                    data.indexOfList = indexOfList;
                    WindowService.instance.ShowCommodityPopup(data);
                    SoundService.instance.Play("tap");
                }
                else
                {
                    ItemPrototype commodityProto = _commodity.commodity;
                    // ItemPrototype itemProto = ItemService.instance.GetPrototype(commodityProto.itemOutPut.id);
                    var data = new ItemPopup.ItemPopupData();
                    data.enableUse = false;
                    data.item = new Item(1, commodityProto.itemOutPut.id);
                    WindowService.instance.ShowItemPopup(data);
                    SoundService.instance.Play("tap");
                }

            }
        }
    }
}
