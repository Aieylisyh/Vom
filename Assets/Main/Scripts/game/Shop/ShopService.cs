using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class ShopService : MonoBehaviour
    {
        public enum ShopCategory
        {
            Normal,
            Payment,
            Merchant,
            Vip,
            Material,
        }

        public static ShopService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public ShopConfig GetShopCfg(ShopCategory shopCategory)
        {
            var c = ConfigService.instance.shopsConfig;
            switch (shopCategory)
            {
                case ShopCategory.Merchant:
                    return c.exchange;

                case ShopCategory.Vip:
                    return c.vip;

                case ShopCategory.Payment:
                    return c.payment;

                case ShopCategory.Normal:
                    return c.normal;

                case ShopCategory.Material:
                    return c.mat;
            }
            return c.normal;
        }

        public ShopCache GetShopCache(ShopCategory shopCategory)
        {
            var gameItemDataCache = UxService.instance.gameItemDataCache;
            ShopCache shopCache = gameItemDataCache.cache.normalSc;
            //因为ShopCache是class是个引用类型，要注意一下
            switch (shopCategory)
            {
                case ShopCategory.Merchant:
                    shopCache = gameItemDataCache.cache.merchantSc;
                    break;

                case ShopCategory.Normal:
                    shopCache = gameItemDataCache.cache.normalSc;
                    break;

                case ShopCategory.Vip:
                    shopCache = gameItemDataCache.cache.vipSc;
                    break;

                case ShopCategory.Payment:
                    shopCache = gameItemDataCache.cache.paymentSc;
                    break;

                case ShopCategory.Material:
                    shopCache = gameItemDataCache.cache.matSc;
                    break;
            }

            return shopCache;
        }

        public List<CommoditySlotData> Fetch(bool toCheck, ShopCategory shopCategory)
        {
            if (toCheck)
                UxService.instance.CheckShopCache();

            return FilterNewCommodities(GetNewCommodities(shopCategory), GetShopCache(shopCategory));
        }

        public string GetIdOfCurrentSlot(ShopCategory shopCategory, int index)
        {
            return GetCommodityCurrentSlot(shopCategory, index).id;
        }

        public ItemPrototype GetCommodityCurrentSlot(ShopCategory shopCategory, int index)
        {
            var newCommodities = GetNewCommodities(shopCategory);
            return newCommodities[index].commodity;
        }

        public List<CommoditySlotData> GetNewCommodities(ShopCategory shopCategory)
        {
            var res = new List<CommoditySlotData>();
            var cache = UxService.instance.gameItemDataCache.cache;
            var cfg = GetShopCfg(shopCategory);
            foreach (var ss in cfg.commodityCfgs)
            {
                CommoditySlotData commodity = null;
                switch (ss.rule)
                {
                    case ShowRule.Order:
                        var refreshCount = UxService.instance.GetShopRefreshCount();
                        //Debug.Log("refreshCount "+refreshCount);
                        var itemCount = ss.items.Count;
                        //Debug.Log("itemCount "+itemCount);
                        var n = refreshCount % itemCount;
                        commodity = ss.items[n];
                        //Debug.Log("commodity "+commodity.commodity.id);
                        break;

                    case ShowRule.Special:
                        foreach (var i in ss.items)
                        {

                            var commodityId = i.commodity.id;
                            //Debug.Log(commodityId);
                            if (commodityId == "C_Diamond_Free_Vip" &&
                                 UxService.instance.gameDataCache.cache.rawLastPlayedDays_freeDiamond != UxService.instance.GetRawPlayedDays())
                            {
                                commodity = i;
                                break;
                            }

                            if (commodityId == "C_Diamond_Ad_Vip")
                            {
                                commodity = i;
                                break;
                            }

                            var plv = UxService.instance.gameDataCache.cache.playerLevel;
                            if (commodityId == "C_Gold_Diamond1" && plv <= 10)
                            {
                                commodity = i;
                                break;
                            }
                            else if (commodityId == "C_Gold_Diamond2" && plv <= 18)
                            {
                                commodity = i;
                                break;
                            }
                            else if (commodityId == "C_Gold_Diamond3" && plv <= 26)
                            {
                                commodity = i;
                                break;
                            }
                            else if (commodityId == "C_Gold_Diamond4" && plv <= 34)
                            {
                                commodity = i;
                                break;
                            }
                            else if (commodityId == "C_Gold_Diamond5" && plv <= 42)
                            {
                                commodity = i;
                                break;
                            }
                            else if (commodityId == "C_Gold_Diamond5" && plv <= 50)
                            {
                                commodity = i;
                                break;
                            }
                            else if (commodityId == "C_Gold_Diamond6")
                            {
                                commodity = i;
                                break;
                            }
                        }
                        break;

                    case ShowRule.Event:
                        int eventCount = UxService.instance.GetEventCount();
                        while (eventCount >= ss.items.Count)
                        {
                            eventCount -= ss.items.Count;
                        }
                        commodity = ss.items[eventCount];
                        break;

                    case ShowRule.Unique:
                        foreach (var i in ss.items)
                        {
                            bool purchased = false;
                            foreach (var purchasedCommodity in UxService.instance.gameItemDataCache.cache.sumSc.Slots)
                            {
                                if (purchasedCommodity.id == i.commodity.id)
                                {
                                    if (ss.items.IndexOf(i) == ss.items.Count - 1 && i.ignoreUniqueRule)
                                    {
                                        purchased = false;//商品不应用unique的刷新显示规则
                                    }
                                    else
                                    {
                                        purchased = true;
                                    }

                                    break;
                                }
                            }

                            if (!purchased)
                            {
                                commodity = i;
                                break;
                            }
                        }
                        break;

                    case ShowRule.First:
                    default:
                        commodity = ss.items[0];
                        break;
                }

                res.Add(commodity);
            }

            return res;
        }

        public List<CommoditySlotData> FilterNewCommodities(List<CommoditySlotData> list, ShopCache sc)
        {
            //Debug.Log("FilterNewCommodities ");
            // com.TextFormat.LogObj(list);
            for (int i = 0; i < list.Count; i++)
            {
                var c = list[i];
                if (c == null)
                    continue;

                foreach (var s in sc.Slots)
                {
                    //Debug.Log("--id " + s);
                    if (s.id == c.commodity.id && s.n > 0)
                    {
                        //Debug.Log("--has cache id "+ s.id);
                        ItemPrototype proto = c.commodity;
                        if (proto.usage == ItemPrototype.Usage.Transaction_limited)
                        {
                            if (c.restockable)
                            {
                                list[i] = ConfigService.instance.shopsConfig.restockData;
                            }
                            else
                            {
                                list[i] = null;
                            }
                        }
                        else if (proto.usage == ItemPrototype.Usage.Transaction_unlimited)
                        {
                            //do nothing if this commodity is Transaction_unlimited 
                            //while has already bought some in this shop refresh
                        }
                        break;
                    }
                }
            }
            return list;
        }
    }
}