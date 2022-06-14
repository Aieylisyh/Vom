using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class ItemService : MonoBehaviour
    {
        public static ItemService instance;

        private void Awake()
        {
            instance = this;
        }

        public ItemPrototype GetPrototype(string id, bool isCommodity = false)
        {
            if (isCommodity)
            {
                foreach (var i in ConfigService.instance.itemConfig.commodityList)
                {
                    if (i.id == id)
                    {
                        return i;
                    }
                }
                return null;
            }

            foreach (var i in ConfigService.instance.itemConfig.list)
            {
                if (i != null && i.id == id)
                {
                    return i;
                }
            }
            return null;
        }

        //if consume==true, will consume the currency
        public TransactionResult IsCommodityAffordable(string commodityId, bool consume, int totalAmount = 1)
        {
            return IsPriceAffordable(GetCommodityPrice(commodityId, totalAmount), consume);
        }

        public List<Item> GetCommodityPrice(string commodityId, int totalAmount = 1)
        {
            var prices = new List<Item>();
            foreach (var price in GetPrototype(commodityId, true).itemValue)
            {
                prices.Add(new Item(price.n * totalAmount, price.id));
            }
            return prices;
        }

        public TransactionResult IsPriceAffordable(List<Item> prices, bool consume)
        {
            foreach (var p in prices)
            {
                var res = IsPriceAffordable(p, false);
                if (!res.success)
                    return res;
            }
            //all ok
            if (consume)
            {
                foreach (var price in prices)
                    UxService.instance.AddItem(price.id, 0 - price.n);
            }

            return new TransactionResult(true);
        }

        public int GetCraftMaxAmount(string craftItemId)
        {
            var proto = GetPrototype(craftItemId, false);
            var needs = proto.itemValue;
            int res = -1;
            foreach (var need in needs)
            {
                var tpHaveCount = UxService.instance.GetItemAmount(need.id);
                int tpRes = Mathf.FloorToInt((float)tpHaveCount / (float)need.n);
                if (res < 0 || res > tpRes)
                {
                    res = tpRes;
                }
            }

            return res;
        }

        public TransactionResult IsPriceAffordable(Item price, bool consume)
        {
            TransactionResult res = new TransactionResult(true);
            var haveCount = UxService.instance.GetItemAmount(price.id);
            var delta = price.n - haveCount;
            var proto = GetPrototype(price.id);
            if (delta > 0)
            {
                res.success = false;
                res.missingId = price.id;
                res.missingTitle = proto.title;
                res.missingString = TextFormat.GetRichTextTag(price.id) + "(" + LocalizationService.instance.GetLocalizedText(proto.title) + ")";
                res.missingAmount = delta;
                res.transactionFailure = TransactionFailure.PriceNonAffordable;
            }
            else
            {
                if (consume)
                    UxService.instance.AddItem(price.id, 0 - price.n);
            }

            return res;
        }

        //update the ShopCache of UxService is not here
        //is in the shop's popup, 
        public TransactionResult DoTransaction(string commodityId, int totalAmount = 1, bool silent = false)
        {
            TransactionResult res = IsCommodityAffordable(commodityId, true, totalAmount);
            //Debug.Log("Transaction id = " + commodityId + "  amount = " + totalAmount + " success=" + res.success);
            if (res.success)
                ClaimTransactionReward(commodityId, silent, totalAmount);
            return res;
        }

        private void ClaimTransactionReward(string commodityId, bool silent, int totalAmount = 1)
        {
            var proto = GetPrototype(commodityId, true);

            var items = new List<Item>();
            items.Add(new Item(proto.itemOutPut.n * totalAmount, proto.itemOutPut.id));
            GiveReward(items, silent, "BuyDoneTitle");
        }

        public void GiveReward(List<Item> rewards, bool silent, string titleKey = "CommonRewardTitle")
        {
            foreach (var reward in rewards)
            {
                UxService.instance.AddItem(reward);
            }

            if (silent)
                return;

            SoundService.instance.Play(new string[3] { "reward", "pay1", "pay2" });
            var data = new ItemsPopup.ItemsPopupData();
            data.clickBgClose = false;
            data.hasBtnOk = true;

            data.title = LocalizationService.instance.GetLocalizedText(titleKey);
            data.content = LocalizationService.instance.GetLocalizedText("CommonRewardContent");
            data.items = rewards;
            WindowService.instance.ShowItemsPopup(data);
        }

        public class TransactionResult
        {
            public TransactionResult(bool suc)
            {
                success = suc;
            }

            public bool success;
            public string missingId;
            public string missingTitle;
            public string missingString;
            public int missingAmount;
            public TransactionFailure transactionFailure;
        }

        public enum TransactionFailure
        {
            PriceNonAffordable,
            ShopInvalid,
            CommodityNotInRegion,
            CommodityNotExist,
        }

        public void ShowTransactionFailConfirmBox(ItemService.TransactionResult res)
        {
            SoundService.instance.Play("btn info");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = false;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            //res.transactionFailure.ToString();
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("Not success");
            if (res.transactionFailure == ItemService.TransactionFailure.PriceNonAffordable)
            {
                confirmBoxData.content = LocalizationService.instance.GetLocalizedTextFormatted("SomethingNotEnough", res.missingString);
            }
            else
            {
                confirmBoxData.content = res.transactionFailure.ToString();
            }
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        public void SellItem(ItemPrototype p, int amountToSell, bool silent)
        {
            List<Item> rewards = new List<Item>();
            foreach (var r in p.itemValue)
            {
                rewards.Add(new Item(r.n * amountToSell, r.id));
            }

            UxService.instance.AddItem(p.id, -amountToSell);
            GiveReward(rewards, silent, "SellDoneTitle");
        }

        public void SellItem(Item p, bool silent)
        {
            var proto = GetPrototype(p.id, false);
            SellItem(proto, p.n, silent);
        }

        public List<Item> MergeItems(Item i, List<Item> itemList)
        {
            var list = new List<Item>();
            list.Add(i);
            return MergeItems(list, itemList);
        }

        public List<Item> MergeItems(List<Item> items, List<Item> itemList)
        {
            var merged = new List<Item>();
            foreach (var item in itemList)
            {
                var newItem = new Item(item.n, item.id);
                foreach (var i in items)
                {
                    if (i.id == item.id)
                    {
                        newItem.n += i.n;
                        items.Remove(i);
                        break;
                    }
                }
                merged.Add(newItem);
            }

            merged.AddRange(items);
            return merged;
        }

        public bool HasFreeRestock()
        {
            return UxService.instance.gameDataCache.cache.restockCount < 5;
        }

        public void OnRestocked()
        {
            UxService.instance.gameDataCache.cache.restockCount += 1;
            UxService.instance.SaveGameData();
        }
    }
}