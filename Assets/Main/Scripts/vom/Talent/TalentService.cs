using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class TalentService
    {
        public static TalentPrototype GetPrototype(string id)
        {
            foreach (var i in ConfigSystem.instance.talentConfig.list)
            {
                if (i.id == id)
                {
                    return i;
                }
            }
            return null;
        }

        public static ItemData GetResetPrice()
        {
            var prices = ConfigSystem.instance.talentConfig.resetDiamondPrices;
            var times = UxService.instance.gameDataCache.cache.resetTalentCount;
            if (times < prices.Count)
            {
                return prices[times];
            }
            return prices[prices.Count - 1];
        }
    }
}
