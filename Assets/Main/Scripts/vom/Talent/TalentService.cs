using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class TalentService : MonoBehaviour
    {
        public static TalentService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public TalentPrototype GetPrototype(string id)
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

        public int GetTalentPoint()
        {
            return UxService.instance.gameDataCache.cache.talentPoint;
        }

        public void SetTalentPoint(int value)
        {
            UxService.instance.gameDataCache.cache.talentPoint = value;
        }

        public LearnTalentResult LearnTalent(string id, bool consume)
        {
            LearnTalentResult res = new LearnTalentResult(true, false, false, false);

            var item = GetItem(id);
            var proto = GetPrototype(id);
            int level = item.saveData.level;
            var crtPoints = GetTalentPoint();

            if (level >= proto.GetMaxLevel())
            {
                res.suc = false;
                res.maxLevelReached = true;
            }
            if (crtPoints <= 0)
            {
                res.suc = false;
                res.noTalentPoint = true;
            }
            if (proto.baseTalents.Count > 0)
            {
                foreach (var baseTalentId in proto.baseTalents)
                {
                    var baseTalentItem = GetItem(baseTalentId);
                    if (baseTalentItem.saveData.level <= 0)
                    {
                        res.suc = false;
                        res.baseTalentNoOk = true;
                        break;
                    }
                }
            }

            if (res.suc && consume)
            {
                item.saveData.level = level + 1;
                SetTalentPoint(crtPoints - 1);
                UxService.instance.SaveGameData();
            }

            return res;
        }

        public void ResetAllTalents()
        {
            var crtPoints = GetTalentPoint();
            var talents = GetItems();
            var releasedPoints = 0;
            foreach (var t in talents)
            {
                releasedPoints += RevertTalent(t.id);
            }

            SetTalentPoint(crtPoints + releasedPoints);
            UxService.instance.SaveGameData();
        }

        public int RevertTalent(string id)
        {
            var item = GetItem(id);
            int level = item.saveData.level;
            item.saveData.level = 0;
            return level;
        }

        public List<TalentItem> GetItems()
        {
            return null;
        }

        public TalentItem GetItem(string id)
        {
            var talents = GetItems();
            foreach (var t in talents)
            {
                //Debug.Log(t);
                if (id == t.id)
                {
                    return t;
                }
            }

            TalentItem newItem = new TalentItem(id);
            talents.Add(newItem);
            UxService.instance.SaveGameData();
            return newItem;
        }

        public int GetAssignedTps()
        {
            int tps = 0;
            var list = ConfigSystem.instance.talentConfig.list;
            foreach (var s in list)
            {
                var item = GetItem(s.id);
                tps += item.saveData.level;
            }
            return tps;
        }

        public int GetAssignedTpsOfCategory(TalentCategory talentCategory)
        {
            int tps = 0;
            var list = ConfigSystem.instance.talentConfig.list;
            foreach (var s in list)
            {
                if (s.category == talentCategory)
                {
                    var item = GetItem(s.id);
                    tps += item.saveData.level;
                }
            }
            return tps;
        }

        public ItemData GetResetPrice()
        {
            var prices = ConfigSystem.instance.talentConfig.resetDiamondPrices;
            var times = UxService.instance.gameDataCache.cache.resetTalentCount;
            if (times < prices.Count)
            {
                return prices[times];
            }
            return prices[prices.Count - 1];
        }

        public struct LearnTalentResult
        {
            public LearnTalentResult(bool p1, bool p2, bool p3, bool p4)
            {
                suc = p1;
                noTalentPoint = p2;
                baseTalentNoOk = p3;
                maxLevelReached = p4;
            }

            public bool suc;
            public bool noTalentPoint;
            public bool baseTalentNoOk;
            public bool maxLevelReached;
        }
    }
}
