using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class CommodityPopup : PopupBehaviour
    {
        public Text title;

        public Text commodityTitle;
        public Text commodityDesc;

        public Text itemTitleWithAmount;
        public Text itemPrice;

        public SlotBehaviour slot;
        public Text btnTxt;
        private CommodityPopupData _data;
        private ItemPrototype _outputItemProto;
        private ItemPrototype _commodityProto;

        public Image bgImage;
        public Image titleImage;
        public Image btnImage;
        public Image slotImage;

        public Sprite bgBlue;
        public Sprite titleBlue;
        public Sprite btnBlue;
        public Sprite slotBlue;

        public Sprite bgGreen;
        public Sprite titleGreen;
        public Sprite btnGreen;
        public Sprite slotGreen;

        public Sprite bgRed;
        public Sprite titleRed;
        public Sprite btnRed;
        public Sprite slotRed;

        public Sprite bgPurple;
        public Sprite titlePurple;
        public Sprite btnPurple;
        public Sprite slotPurple;

        public struct CommodityPopupData
        {
            public CommoditySlotData commodity;
            public ShopService.ShopCategory shopCategory;
            public int indexOfList;
        }

        public void Setup(CommodityPopupData data)
        {
            _data = data;

            slot.Setup(data.commodity);
            _commodityProto = data.commodity.commodity;
            _outputItemProto = ItemService.instance.GetPrototype(_commodityProto.itemOutPut.id);

            var outputItemTitleString = LocalizationService.instance.GetLocalizedText(_outputItemProto.title);
            var commodityDescString = LocalizationService.instance.GetLocalizedText(_commodityProto.desc);
            commodityDesc.text = commodityDescString;


            var isRestock = _commodityProto.id == ConfigService.instance.shopsConfig.restockData.commodity.id;
            if (isRestock)
            {
                title.text = LocalizationService.instance.GetLocalizedText("Restock");
                commodityTitle.text = "";
                commodityDesc.text = LocalizationService.instance.GetLocalizedText("Restock_subDesc");
                btnTxt.text = LocalizationService.instance.GetLocalizedText("Restock");

                var toRestock = ShopService.instance.GetCommodityCurrentSlot(_data.shopCategory, _data.indexOfList);
                itemTitleWithAmount.text = LocalizationService.instance.GetLocalizedText("Restock_desc") + ": " + TextFormat.GetItemText(new Item(toRestock.itemOutPut.n, toRestock.itemOutPut.id), true);

                if (ItemService.instance.HasFreeRestock())
                {
                    itemPrice.text = LocalizationService.instance.GetLocalizedText("Cost") + ": " + LocalizationService.instance.GetLocalizedText("Free");
                }
                else
                {
                    var restockPrice = _commodityProto.itemValue;
                    itemPrice.text = LocalizationService.instance.GetLocalizedText("Cost") + ": " + TextFormat.GetItemText(restockPrice, true);
                }

                bgImage.sprite = bgGreen;
                titleImage.sprite = titleGreen;
                btnImage.sprite = btnGreen;
                slotImage.sprite = slotGreen;
            }
            else
            {
                itemTitleWithAmount.text = LocalizationService.instance.GetLocalizedText("Get") + ": " + TextFormat.GetItemText(new Item(_commodityProto.itemOutPut.n, _outputItemProto.id), true);

                if (data.shopCategory == ShopService.ShopCategory.Merchant)
                {
                    commodityTitle.text = LocalizationService.instance.GetLocalizedTextFormatted("ExchangeCommodity", outputItemTitleString);
                    title.text = LocalizationService.instance.GetLocalizedText("Exchange");
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Ok");
                    bgImage.sprite = bgGreen;
                    titleImage.sprite = titleGreen;
                    btnImage.sprite = btnGreen;
                    slotImage.sprite = slotGreen;

                    itemPrice.text = LocalizationService.instance.GetLocalizedText("Need") + ": " + TextFormat.GetItemText(_commodityProto.itemValue, true);
                    if (string.IsNullOrEmpty(commodityDescString))
                        commodityDesc.text = ((_commodityProto.usage == ItemPrototype.Usage.Transaction_limited) ? "" : LocalizationService.instance.GetLocalizedText("Unlimited Exchange"));
                }
                else if (data.shopCategory == ShopService.ShopCategory.Payment)
                {
                    commodityTitle.text = LocalizationService.instance.GetLocalizedTextFormatted("PurchaseCommodity", outputItemTitleString);
                    title.text = LocalizationService.instance.GetLocalizedText("Purchase");
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Ok");
                    bgImage.sprite = bgPurple;
                    titleImage.sprite = titlePurple;
                    btnImage.sprite = btnPurple;
                    slotImage.sprite = slotPurple;
                    FetchIapInfo(data);
                }
                else
                {
                    if (data.commodity.isFree)
                    {
                        commodityTitle.text = LocalizationService.instance.GetLocalizedTextFormatted("FreeCommodity", outputItemTitleString);
                    }
                    else if (data.commodity.hasAd)
                    {
                        commodityTitle.text = LocalizationService.instance.GetLocalizedTextFormatted("AdCommodity", outputItemTitleString);
                    }
                    else
                    {
                        commodityTitle.text = LocalizationService.instance.GetLocalizedTextFormatted("BuyCommodity", outputItemTitleString);
                    }
                    title.text = LocalizationService.instance.GetLocalizedText("Buy");
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Ok");

                    if (data.shopCategory == ShopService.ShopCategory.Merchant && !HasVip())
                    {
                        bgImage.sprite = bgRed;
                        titleImage.sprite = titleRed;
                        btnImage.sprite = btnRed;
                        slotImage.sprite = slotRed;
                    }
                    else
                    {
                        bgImage.sprite = bgBlue;
                        titleImage.sprite = titleBlue;
                        btnImage.sprite = btnBlue;
                        slotImage.sprite = slotBlue;
                    }

                    if (string.IsNullOrEmpty(commodityDescString))
                        commodityDesc.text = ((_commodityProto.usage == ItemPrototype.Usage.Transaction_limited) ? "" : LocalizationService.instance.GetLocalizedText("Unlimited Transaction"));

                    if (data.commodity.isFree)
                    {
                        itemPrice.text = LocalizationService.instance.GetLocalizedText("Free");
                    }
                    else if (data.commodity.hasAd)
                    {
                        itemPrice.text = LocalizationService.instance.GetLocalizedText("Ad");
                    }
                    else
                    {
                        itemPrice.text = LocalizationService.instance.GetLocalizedText("Cost") + ": " + TextFormat.GetItemText(_commodityProto.itemValue, true);
                    }
                }
            }
        }

        void FetchIapInfo(CommodityPopupData data)
        {
            Debug.Log("FetchIapInfo");
            var service = PayService.instance;
            var commodityProto = _data.commodity.commodity;
            var product = service.GetIapProduct(service.GetIapIdByCommodity(commodityProto.id));
            //Debug.Log("这里要从meta data获取价格等iap信息");
            //PayService.LogProduct(product);

            if (product == null)
            {
                //找不到信息
                itemPrice.text = "...";
            }
            else
            {
                itemPrice.text = LocalizationService.instance.GetLocalizedText("Price") + ": " + product.metadata.localizedPriceString;
                //itemPrice.text = "";
            }
        }

        private bool HasVip()
        {
            return UxService.instance.GetRestTimeVip().Ticks > 0;
        }

        private bool IsIapOk()
        {
            if (ConfigService.instance.payConfig.testMode)
            {
                return true;
            }

            var service = PayService.instance;
            if (!service.isEnabled())
            {
                return false;
            }

            var commodityProto = _data.commodity.commodity;
            var product = service.GetIapProduct(service.GetIapIdByCommodity(commodityProto.id));
            if (product == null)
            {
                return false;
            }

            return true;
        }

        void NoVipInfo()
        {
            //vip limit forbidden logic
            SoundService.instance.Play("btn info");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = false;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("NeedVipTitle");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("NeedVipDesc");
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        void NoIapInfo()
        {
            //vip limit forbidden logic
            SoundService.instance.Play("btn info");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = false;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("NoIapTitle");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("NoIapDesc");
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        public void OnClickBuyBtn()
        {
            //Debug.Log("commodity popup OnClickBuyBtn");
            if (_data.shopCategory == ShopService.ShopCategory.Payment && !IsIapOk())
            {
                Hide();
                NoIapInfo();
                return;
            }

            if (_data.shopCategory == ShopService.ShopCategory.Vip && !HasVip())
            {
                Hide();
                NoVipInfo();
                return;
            }

            Sound();
            switch (_commodityProto.usage)
            {
                case ItemPrototype.Usage.Transaction_limited:
                    DoTransaction(1);
                    Hide();
                    break;

                case ItemPrototype.Usage.Restock:
                    TryRestock();
                    break;

                case ItemPrototype.Usage.Iap:
                    WaitingCircleBehaviour.instance.SetHideAction();
                    WaitingCircleBehaviour.instance.Show(300f);
                    TryIap();
                    break;

                case ItemPrototype.Usage.Transaction_unlimited:
                    var commodityProto = _data.commodity.commodity;
                    var prices = commodityProto.itemValue;
                    int max = 0;
                    foreach (var price in prices)
                    {
                        int amount = UxService.instance.GetItemAmount(price.id);
                        if (amount <= 0)
                        {
                            max = 0;
                        }
                        else
                        {
                            int tpMax = amount / price.n;
                            if (max == 0 || max > tpMax)
                            {
                                max = tpMax;
                            }
                        }
                    }
                    if (max < 2)
                    {
                        DoTransaction(1);
                        Hide();
                    }
                    else
                    {
                        WindowService.instance.ShowAmountSelectPopup(_data.commodity, this, max);
                    }
                    break;
            }
        }

        void TryIap()
        {
            var service = PayService.instance;
            var commodityProto = _data.commodity.commodity;
            //这里的商品id是配置的item的id 不是iap的id 需要通过我自己的payconfig转化

            var iapid = service.GetIapIdByCommodity(commodityProto.id);
            if (ConfigService.instance.payConfig.testMode)
            {
                Debug.Log("skipped Iap");
                Hide();
                DoTransactionSync(commodityProto.id, 1);
                WaitingCircleBehaviour.instance.Hide();
                return;
            }

            service.OnPurchaseClicked(iapid,
                () =>
                {
                    Hide();
                    DoTransactionSync(commodityProto.id, 1);
                    WaitingCircleBehaviour.instance.Hide();
                },
                () =>
                {
                    Hide();
                    NoIapInfo();
                    WaitingCircleBehaviour.instance.Hide();
                }
            );
        }

        void TryRestock()
        {
            var commodityProto = _data.commodity.commodity;
            var prices = commodityProto.itemValue;
            var res = ItemService.instance.IsPriceAffordable(prices, true);
            if (ItemService.instance.HasFreeRestock())
            {
                res.success = true;
            }

            if (res.success)
            {
                SoundService.instance.Play("btn big");
                ItemService.instance.OnRestocked();
                var toRestockId = ShopService.instance.GetIdOfCurrentSlot(_data.shopCategory, _data.indexOfList);
                UxService.instance.RemoveShopCache(_data.shopCategory, toRestockId);
                Hide();
                WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
            }
            else
            {
                ItemService.instance.ShowTransactionFailConfirmBox(res);
            }
        }

        public void OnAmountSelected(int amount)
        {
            //Debug.Log("OnAmountSelected " + amount);
            DoTransaction(amount);
            Hide();
        }

        public void DoTransaction(int amount = 1)
        {
            if (amount < 1)
                return;

            var commodityId = _data.commodity.commodity.id;
            var prices = ItemService.instance.GetCommodityPrice(commodityId, amount);

            if ((prices.Count > 0) && prices[0].id == "Ad")
            {
                AdService.instance.PlayAd(
                () =>
                {
                    Debug.Log("ad fail, buy");
                    AdService.instance.CommonFeedback_Fail();
                },
                () =>
                {
                    Debug.Log("ad suc, buy");
                    DoTransactionSync(commodityId, amount);
                    AdService.instance.CommonFeedback_Suc();
                },
                () =>
                {
                    Debug.Log("ad cease, buy");
                    AdService.instance.CommonFeedback_Cease();
                },
                () =>
                {
                    Debug.Log("ad canplay, buy");

                },
                () =>
                {
                    Debug.Log("ad can not play, buy");
                    AdService.instance.CommonFeedback_CanNotPlay();
                }
                );
                return;
            }

            DoTransactionSync(commodityId, amount);
        }

        //ad and free is instant suc here
        void DoTransactionSync(string commodityId, int amount)
        {
            var res = ItemService.instance.DoTransaction(commodityId, amount);
            if (!res.success)
            {
                ItemService.instance.ShowTransactionFailConfirmBox(res);
                return;
            }

            //buy suc
            //Debug.Log("buy suc " + commodityId);
            if (commodityId == "C_Diamond_Free_Vip")
            {
                UxService.instance.gameDataCache.cache.rawLastPlayedDays_freeDiamond = UxService.instance.GetRawPlayedDays();
            }

            UxService.instance.UpdateShopCache(_data, commodityId, amount);
            MainHudBehaviour.instance.Refresh();
            UxService.instance.SaveGameItemData();
            UxService.instance.SaveGameData();
        }
    }
}
