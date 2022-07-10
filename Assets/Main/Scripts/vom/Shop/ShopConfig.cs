using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class ShopConfig : ScriptableObject
    {
        public List<ShopSlot> commodityCfgs;
    }

    [System.Serializable]
    public class ShopSlot
    {
        public List<CommoditySlotData> items;
        public ShowRule rule;
    }

    public enum ShowRule
    {
        First,
        Event,
        Order,//change every ShopRefresh
        Unique,//Check Item show next available
        Special,//switch each item
    }

    [System.Serializable]
    public class CommoditySlotData
    {
        public ItemPrototype commodity;
        public bool isPayment = false;
        public bool hasAd = false;
        public bool isFree = false;
        public bool restockable = false;
        public bool disablePostBuyRefresh = false;
        public bool ignoreUniqueRule = false;
    }
}