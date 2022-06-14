using com;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace game
{
    public class LevelIdleService : MonoBehaviour
    {
        public static LevelIdleService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void TryClaimReward()
        {
            var validMinutes = GetValidMinutes();
           // Debug.Log("TryClaimReward validMinutes "+ validMinutes);
            if (validMinutes < 1)
            {
                FloatingTextPanelBehaviour.instance.Create(LocalizationService.instance.GetLocalizedText("MNI_IdleNotEnough"), 0.5f, 0.65f);
                return;
            }

            GiveIdleRewards(validMinutes);
            ResetTime();
            MapWindowBehaviour.instance.map.ForceTick();
        }

        public int GetValidMinutes()
        {
            //Debug.Log(delta.Seconds + " " + delta.TotalSeconds);
            //timespan的second和totalsecond的区别：
            //前者是floor的取整
            //前者一分钟后从0开始，后者一直加
            var delta = GetDeltaTime();
            var validMinutes = Mathf.FloorToInt((float)delta.TotalMinutes);
            var totalMinutes = ConfigService.instance.levelConfig.maxIdleHours * 60;
            //Debug.Log("validMinutes " + (float)delta.TotalMinutes);
            //Debug.Log("totalMinutes " + totalMinutes);
            if (validMinutes > totalMinutes)
            {
                validMinutes = totalMinutes;
            }
            return validMinutes;
        }

        public float GetIdleTimePercent()
        {
            var validMinutes = GetValidMinutes();
            var totalMinutes = ConfigService.instance.levelConfig.maxIdleHours * 60;
            float res = (float)validMinutes / totalMinutes;
            //Debug.Log("GetIdleTimePercent " + res);
            return res;
        }

        void GiveIdleRewards(int validMinutes)
        {
            //Debug.Log("GiveIdleRewards  validMinutes " + validMinutes);
            var rewards = GetIdleRewards(validMinutes);
            foreach (var reward in rewards)
            {
                UxService.instance.AddItem(reward);
            }

            SoundService.instance.Play(new string[3] { "reward", "pay1", "pay2" });
            var data = new ItemsPopup.ItemsPopupData();
            data.clickBgClose = false;
            data.hasBtnOk = true;
            var timeString = TextFormat.GetRestTimeStringFormated(TimeSpan.FromMinutes(validMinutes));
            data.title = LocalizationService.instance.GetLocalizedText("MNI_IdleRewardTitle");
            data.content = LocalizationService.instance.GetLocalizedTextFormatted("MNI_IdleRewardContent", timeString);
            data.items = rewards;
            WindowService.instance.ShowItemsPopup(data);
        }

        List<Item> GetIdleRewards(int validMinutes)
        {
            //Debug.Log("GetIdleRewards " + validMinutes);
            var res = new List<Item>();
            var cfg = ConfigService.instance.levelConfig;
            var stars = LevelService.instance.GetStarCount();
            var rewardCount = cfg.idleRewardCountPerMinute.GetIntValue(stars) * validMinutes;

            for (var i = 0; i < rewardCount; i++)
            {
                var index = UnityEngine.Random.Range(0, cfg.idleRewards.Count);
                var item = cfg.idleRewards[index];
                var parsedRes = ConfigService.instance.itemConfig.ParseComplexItem(item.id, item.n);
                if (parsedRes != null && parsedRes.Count > 0)
                {
                    foreach (var parsed in parsedRes)
                    {
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

        void ResetTime()
        {
            //Debug.Log("ResetTime");
            var delta = GetDeltaTime();
           // Debug.Log("GetDeltaTime TotalMinutes " + delta.TotalMinutes);
            var restMinutes = delta.TotalMinutes - Mathf.Floor((float)delta.TotalMinutes);
            TimeSpan deltaM = TimeSpan.FromMinutes(restMinutes);
            //Debug.Log("GetDeltaTime deltaM TotalMinutes " + deltaM.TotalMinutes);
            UxService.instance.gameDataCache.cache.idleClaimedDate = DateTime.Now - deltaM;
            UxService.instance.SaveGameData();
        }

        TimeSpan GetDeltaTime()
        {
            var delta = DateTime.Now - UxService.instance.gameDataCache.cache.idleClaimedDate;
            return delta;
        }
    }
}