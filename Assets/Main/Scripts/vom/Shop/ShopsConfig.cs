using UnityEngine;

namespace vom
{
    [CreateAssetMenu]
    public class ShopsConfig : ScriptableObject
    {
        public ShopConfig normal;
        public ShopConfig mat;
        public ShopConfig payment;
        public ShopConfig exchange;
        public ShopConfig vip;

        public CommoditySlotData restockData;
    }
}