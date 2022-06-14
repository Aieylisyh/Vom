using UnityEngine;
using System.Collections.Generic;
using com;
using System;

namespace game
{
    public class FishingService : MonoBehaviour
    {
        public static FishingService instance;

        public int sessionAdAccUsedCount;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            sessionAdAccUsedCount = 0;
        }

        public FishingItem GetItem()
        {
            return UxService.instance.gameDataCache.cache.fishingItem;
        }

        public bool HasRft()
        {
            var item = GetItem();
            return item.saveData.hasRft;
        }

        public bool HasFinishedRft()
        {
            if (!HasRft())
                return false;

            var ts = GetRftRestTimeSpan();
            if (ts.Ticks > 0)
                return false;

            return true;
        }

        public void FinishRft()
        {
            var item = GetItem();
            //Debug.Log("-FinishRft n." + item.saveData.rftIndex);
            if (!item.saveData.hasRft)
            {
                Debug.LogError("FinishRft hasRft==false");
                return;
            }

            item.saveData.hasRft = false;
            item.saveData.rftIndex = item.saveData.rftIndex + 1;
            UxService.instance.SaveGameData();
        }

        public FishingPrototype GetPrototype()
        {
            var item = GetItem();
            var cfg = GetConfig();
            var i = item.saveData.rftIndex;
            foreach (var p in cfg.rftPrototypes)
            {
                if (p.tutoIndex == i)
                {
                    return p;
                }
            }

            return cfg.defaultRftPrototype;
        }

        public bool HasBoatLevelupUnlocked()
        {
            var item = GetItem();
            var rftIndex = item.saveData.rftIndex;
            return rftIndex >= ConfigService.instance.tutorialConfig.minRftIndexEnableFunctionsData.levelup;
        }

        public bool HasBoatAbilityUnlocked()
        {
            var item = GetItem();
            var rftIndex = item.saveData.rftIndex;
            return rftIndex >= ConfigService.instance.tutorialConfig.minRftIndexEnableFunctionsData.fab;
        }

        public FishingConfig GetConfig()
        {
            return ConfigService.instance.fishingConfig;
        }

        public bool IsLevelupPossible()
        {
            var cfg = GetConfig();
            if (GetBoatLevel() >= cfg.boatLevelData.maxLevel)
            {
                return false;
            }

            return true;
        }

        public bool IsLevelupAffordable(bool afford)
        {
            var item = GetItem();
            var cfg = GetConfig();
            var res = true;
            var affordableRes = ItemService.instance.IsPriceAffordable(GetLevelupPrice(), afford);
            if (!affordableRes.success)
            {
                res = false;
            }

            return res;
        }

        public int GetBoatLevel()
        {
            var item = GetItem();
            return item.saveData.boatLevel;
        }

        public Item GetLevelupPrice()
        {
            var cfg = GetConfig();
            var priceNum = cfg.boatLevelData.goldCost.GetIntValue(GetBoatLevel());
            return new Item(priceNum, "Gold");
        }

        public bool IsAbilityLearnPossible(FishingAbilityUnlockPrototype abu)
        {
            var res = false;
            var item = GetItem();
            int levelReq = abu.boatLevelRequire;
            //if (abu == null) abu = GetAbilityUnlock(proto, GetLastUnlockedAbilityUnlockIndex(item));//must not prevent this case

            if (item.saveData.boatLevel >= levelReq)
                res = true;

            return res;
        }

        public int GetLastUnlockedAbilityUnlockIndex()
        {
            Debug.LogError("should never use this");
            var proto = GetPrototype();
            var item = GetItem();
            var cfg = GetConfig();
            if (item.saveData.unlockedAbilities.Count < cfg.abilityUnlocks.Count)
            {
                //not all unlocked
                return item.saveData.unlockedAbilities.Count;
            }
            // all unlocked
            return -1;//not exist!
        }

        public FishingAbilityUnlockPrototype GetAbilityUnlockPrototype(string id)
        {
            var cfg = GetConfig();
            foreach (var abu in cfg.abilityUnlocks)
            {
                if (abu.ability.id == id)
                {
                    return abu;
                }
            }

            return null;
        }

        public FishingAbilityPrototype GetAbilityPrototype(string id)
        {
            var abu = GetAbilityUnlockPrototype(id);
            if (abu != null)
            {
                return abu.ability;
            }

            return null;
        }

        public bool IsAbilityLearnAffordable(FishingAbilityUnlockPrototype abu)
        {
            var res = true;
            var affordableRes = ItemService.instance.IsPriceAffordable(abu.price, false);
            if (!affordableRes.success)
            {
                res = false;
            }

            return res;
        }

        public void LevelupBoat()
        {
            var item = GetItem();
            item.saveData.boatLevel = item.saveData.boatLevel + 1;
            UxService.instance.SaveGameData();
        }

        public void UnlockFishingAbility(string s)
        {
            var item = GetItem();
            item.saveData.unlockedAbilities.Add(s);
            UxService.instance.SaveGameData();
        }

        public bool HasUnlockedAbility(string abilityId)
        {
            var item = GetItem();
            foreach (var ab in item.saveData.unlockedAbilities)
            {
                if (ab == abilityId)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Item> GetRftRewards()
        {
            var res = new List<Item>();
            if (!HasRft())
            {
                return res;
            }

            var proto = GetPrototype();
            var cfg = GetConfig();
            var totalAmount = GetRftAmount();

            var amountAllos = ListUtil.FastRandomAllocate(totalAmount, GetRftPickWeights());
            for (int i = 0; i < cfg.fishToPick.Count; i++)
            {
                var amountAllo = amountAllos[i];
                if (amountAllo > 0)
                {
                    res.Add(new Item(amountAllo, cfg.fishToPick[i]));
                }
            }

            if (proto.extraReward != null)
            {
                res = ItemService.instance.MergeItems(proto.extraReward, res);
            }

            var extra = GetExtraFishingItems();
            //ListUtil.LogList(extra);
            res = ItemService.instance.MergeItems(extra, res);

            return res;
        }

        public List<Item> GetRftExtraRewards(float ratio)
        {
            var totalAmount = GetRftAmountRaw() * ratio;
            var res = new List<Item>();
            var proto = GetPrototype();
            var cfg = GetConfig();
            var amountAllos = ListUtil.FastRandomAllocate((int)totalAmount, GetRftPickWeights());
            for (int i = 0; i < cfg.fishToPick.Count; i++)
            {
                var amountAllo = amountAllos[i];
                if (amountAllo > 0)
                {
                    res.Add(new Item(amountAllo, cfg.fishToPick[i]));
                }
            }
            var extra = GetExtraFishingItems(ratio);
            res = ItemService.instance.MergeItems(extra, res);
            return res;
        }

        public List<Item> GetExtraFishingItems(float ratio = 1)
        {
            var res = new List<Item>();
            var cfg = GetConfig();
            var level = GetBoatLevel();
            var proto = GetPrototype();

            int num2 = cfg.boatLevelData.rftAmount.GetIntValue(level);
            int num1 = cfg.boatLevelData.rftAmount.GetIntValue(0);
            float bonus = (float)num2 / (float)num1 * 0.5f;

            int pick = Mathf.FloorToInt(cfg.extraPickWeight * bonus * ratio);
            //Debug.Log("!GetExtraFishingItems bonus " + bonus + " pick " + pick);
            for (var i = 0; i < pick; i++)
            {
                var index = UnityEngine.Random.Range(0, cfg.extraItems.Count);
                var item = cfg.extraItems[index];
                //Debug.Log(item.id);
                var parsedRes = ConfigService.instance.itemConfig.ParseComplexItem(item.id, item.n);
                if (parsedRes != null && parsedRes.Count > 0)
                {
                    foreach (var parsed in parsedRes)
                    {
                        //Debug.Log(parsed.id);
                        res = ItemService.instance.MergeItems(new Item(parsed.n, parsed.id), res);
                    }
                }
                else
                {
                    res = ItemService.instance.MergeItems(new Item(item.n, item.id), res);
                }
            }

            return res;
        }

        public int GetRftFabBonus()
        {
            int res = 0;
            if (HasUnlockedAbility("Fab5"))
                res += GetAbilityPrototype("Fab5").intParam;

            return res;
        }

        public int GetRftAmount()
        {
            //Debug.Log("GetRftAmount");
            var otvt = GetOvertimeValidateLength();
            if (otvt <= 0)
            {
                return GetRftAmountRaw();
            }
            //Debug.Log("otvt " + otvt);
            //Debug.Log(MathGame.TimeSpanTicksToSeconds(otvt));
            return GetRftAmountWithOvertime(otvt);
        }

        public int GetRftAmountRaw()
        {
            var cfg = GetConfig();
            var level = GetBoatLevel();
            var proto = GetPrototype();

            int num = cfg.boatLevelData.rftAmount.GetIntValue(level);
            var bonus = GetRftFabBonus();
            num = (int)MathGame.GetPercentageAdded(proto.rewardRatio * num, bonus);
            return num;
        }

        public int GetRftAmountWithOvertime(long overtimeValidateLength)
        {
            //Debug.Log("GetRftAmountWithOvertime " + overtimeValidateLength);
            var num = GetRftAmountRaw();
            //Debug.Log(num);
            var d = GetRftDuration_TimeSpanTicks();
            //Debug.Log(overtimeValidateLength);
            // Debug.Log(d);
            float ratio = (float)overtimeValidateLength / (float)d;
            //Debug.Log(ratio);
            num = (int)(num * (1f + ratio));
            //Debug.Log(num);
            return num;
        }

        public string GetRftAmountEstimationString()
        {
            var es = GetRftAmountEstimation();
            string res = "" + es[0];
            if (es[1] > 0)
                res += "<color=#BBFFA0> +" + es[1] + "</color>";

            return res;
        }
        /// <summary>
        /// 1 before add amount 2 add amount
        /// </summary>
        /// <returns>amount bonus</returns>
        public List<int> GetRftAmountEstimation()
        {
            var cfg = GetConfig();
            var level = GetBoatLevel();
            var proto = GetPrototype();

            int num = cfg.boatLevelData.rftAmount.GetIntValue(level);
            var bonus = GetRftFabBonus();
            int baseAmount = (int)(num * proto.rewardRatio);
            int extraAmount = (int)(num * (bonus * 0.01f));

            var res = new List<int>();
            res.Add(baseAmount);
            res.Add(extraAmount);
            return res;
        }

        public float GetRftDuration()
        {
            var cfg = GetConfig();
            var level = GetBoatLevel();
            var proto = GetPrototype();
            //Debug.Log("proto.durationSeconds " + proto.durationSeconds);
            int bonus = cfg.boatLevelData.rftDurationAddPercent.GetIntValue(level);
            //Debug.Log("GetRftDuration bonus " + bonus);
            var RftDurationInSec = MathGame.GetPercentageAdded(proto.durationSeconds, bonus);
            //Debug.Log("RftDurationInSec " + RftDurationInSec);
            return RftDurationInSec;
        }

        public long GetRftDuration_TimeSpanTicks()
        {
            return MathGame.SecondToTimeSpanTicks(GetRftDuration());
        }

        public List<int> GetRftPickWeights()
        {
            var res = new List<int>();
            var cfg = GetConfig();
            for (int i = 0; i < cfg.fishToPick.Count; i++)
            {
                bool available = false;
                var fishToPick = cfg.fishToPick[i];
                if (cfg.defaultFish.Contains(fishToPick))
                {
                    available = true;
                }
                else if ((fishToPick == "Fish3" || fishToPick == "Fish4")
                  && HasUnlockedAbility("Fab1"))
                {
                    available = true;
                }
                else if ((fishToPick == "Fish5" || fishToPick == "Fish6")
                && HasUnlockedAbility("Fab4"))
                {
                    available = true;
                }

                res.Add(available ? cfg.fishPickWeight[i] : 0);
            }

            return res;
        }

        public long GetOvertimeMaxHours()
        {
            long res = 0;
            if (HasUnlockedAbility("Fab2"))
                res += GetAbilityPrototype("Fab2").intParam;
            if (HasUnlockedAbility("Fab7"))
                res += GetAbilityPrototype("Fab7").intParam;

            return res;
        }

        public long GetOvertimeMax_TimeSpanTicks()
        {
            var h = GetOvertimeMaxHours();
            //Debug.Log("GetOvertimeMaxHours " + h);
            return MathGame.SecondToTimeSpanTicks(h * 3600);
        }

        public int GetOvertimeEfficiencyPercent()
        {
            return 100;
        }

        public long GetOvertimeValidateLength()
        {
            var eff = GetOvertimeEfficiencyPercent();
            if (eff <= 0)
                return 0;

            var flTime = GetOvertimeValidatedDurationTicks();
            return flTime * (eff / (long)100);
        }

        public long GetOvertimeValidatedDurationTicks()
        {
            var flMax = GetOvertimeMax_TimeSpanTicks();
            if (flMax <= 0)
            {
                return 0;
            }

            var now = DateTime.Now;
            var item = GetItem();
            var flEndTime = item.saveData.flStartDate + GetRftDuration_TimeSpanTicks();

            var deltaTicks = now.Ticks - flEndTime;
            //Debug.Log("deltaTicks " + deltaTicks);
            if (deltaTicks <= 0)
            {
                return 0;
            }

            var flTime = deltaTicks;
            if (flTime > flMax)
            {
                flTime = flMax;
            }

            return flTime;
        }

        public void TryShowBoat()
        {
            var item = GetItem();
            if (!HasRft())
            {
                FishingBoatBehaviour.instance.SetInPort();
                return;
            }

            FishingBoatBehaviour.instance.SetInOcean();
            //boat will enter port by FishingBoatBehaviour tick
        }

        public bool CanAccRft()
        {
            var inOcean = FishingBoatBehaviour.instance.IsInOcean();
            if (!inOcean)
            {
                return false;
            }

            if (!HasRft())
            {
                return false;
            }

            var proto = GetPrototype();
            if (!proto.canAcc)
            {
                return false;
            }

            var cfg = GetConfig();
            var restTimeSpan = GetRftRestTimeSpan();
            //Debug.Log("restTimeSpan.TotalSeconds " + restTimeSpan.TotalSeconds);
            if (restTimeSpan.TotalSeconds <= cfg.noAccTimeSec)
            {
                return false;
            }

            return true;
        }

        public TimeSpan GetRftRestTimeSpan()
        {
            var item = GetItem();
            var now = DateTime.Now;
            var flEndTime = item.saveData.flStartDate + GetRftDuration_TimeSpanTicks();
            //Debug.Log(flEndTime);
            var deltaTicks = flEndTime - now.Ticks;
            //Debug.Log("deltaTicks " + deltaTicks);
            var deltaTimeSpan = TimeSpan.FromTicks(deltaTicks);
            return deltaTimeSpan;
        }

        public void StartRft()
        {
            var cfg = GetConfig();
            var item = GetItem();
            item.saveData.hasRft = true;
            item.saveData.flStartDate = DateTime.Now.Ticks;
            Debug.LogWarning("StartRft " + item.saveData.rftIndex);
            //Debug.LogWarning("flStartDate " + item.saveData.flStartDate);
            FishingBoatBehaviour.instance.LeavePort();
            UxService.instance.SaveGameData();
            FishingWindowBehaviour.instance.OnBoatLeave();
        }

        public void AccRft()
        {
            var item = GetItem();
            long durationTicks = GetRftDuration_TimeSpanTicks();
            long flNow = DateTime.Now.Ticks;

            var cfg = GetConfig();
            float accReturnSec = cfg.accReturnSecond;
            item.saveData.flStartDate = flNow - durationTicks + MathGame.SecondToTimeSpanTicks(accReturnSec);

            UxService.instance.SaveGameData();
        }

        public List<Item> GetAccPrice()
        {
            var res = new List<Item>();
            var timeSpan = GetRftRestTimeSpan();
            var sec = timeSpan.TotalSeconds;
            var cfg = GetConfig();

            float validSec = (float)sec - cfg.noAccTimeSec;
            if (validSec <= 0)
                return res;

            var secPerDiamond = cfg.accTimeSecPerDiamond;
            if (HasUnlockedAbility("Fab3"))
                secPerDiamond = MathGame.GetPercentageAdded(secPerDiamond, 66);
            if (HasUnlockedAbility("Fab6"))
                secPerDiamond = MathGame.GetPercentageAdded(secPerDiamond, 66);
            var diamondCount = Mathf.CeilToInt(validSec / secPerDiamond);

            if (validSec < cfg.freeAccTimeSec)
            {
                diamondCount = 0;
            }

            if (diamondCount > 0)
            {
                if (CanUseAdAcc(diamondCount))
                {
                    res.Add(new Item(1, "Ad"));
                }
                else
                {
                    res.Add(new Item(diamondCount, "Diamond"));
                }
            }
            else
            {
                res.Add(new Item(1, "Free"));
            }

            return res;
        }

        public bool CanUseAdAcc(int diamondCost)
        {
            if (!AdService.instance.CanPlayAd(false))
            {
                return false;
            }

            var cfg = GetConfig();
            if (diamondCost >= cfg.adAccData.lowCostCap && diamondCost <= cfg.adAccData.highCostCap)
            {
                if (sessionAdAccUsedCount < cfg.adAccData.countPerSession)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
