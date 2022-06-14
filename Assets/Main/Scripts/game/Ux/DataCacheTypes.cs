using System;
using System.Collections.Generic;

namespace game
{
    [Serializable]
    public class SettingsData
    {
        public SettingsData()
        {
            Reset();
        }

        public void Reset()
        {
            var cfg = ConfigService.instance.settingsConfig;
            sfxOn = cfg.sfxOn;
            musicOn = cfg.musicOn;
            vibrateOn = cfg.vibrateOn;
            language = cfg.languageDefault;
            langueHasSet = false;
        }

        public bool sfxOn;
        public bool musicOn;
        public bool vibrateOn;

        public com.LocalizationService.Language language
        {
            get
            {
                return com.LocalizationService.GetLanguageByString(languageStr);
            }
            set
            {
                languageStr = com.LocalizationService.GetLanguageString(value);
            }
        }

        public string languageStr;
        public bool langueHasSet;
    }

    [System.Serializable]
    public class AccountData
    {
        public AccountData()
        {
            Reset();
        }

        public void Reset()
        {
            slotId = "";
            loginId = "zqt";//not used now
            firstLaunchDate = DateTime.Now;
            lastLaunchDate = DateTime.Now;
        }

        public string slotId;
        public string loginId;
        public long flDate;
        public long llDate;
        public DateTime firstLaunchDate
        {
            get
            {
                return new DateTime(flDate);
            }
            set
            {
                flDate = value.Ticks;
            }
        }
        public System.DateTime lastLaunchDate
        {
            get
            {
                return new DateTime(llDate);
            }
            set
            {
                llDate = value.Ticks;
            }
        }
    }

    [System.Serializable]
    public class GameItemData
    {
        public List<Item> items;

        public List<MailItem> mails;

        public int shopRefreshCount;

        public ShopCache paymentSc;//temp paymentShopCache
        public ShopCache merchantSc;//temp merchantShopCache
        public ShopCache vipSc;//temp vipShopCache
        public ShopCache normalSc;//temp normalShopCache
        public ShopCache matSc;//temp material ShopCache
        public ShopCache sumSc;//purchased Commodities combined, not temp, can be used to determind things like Course1 whether ever proceeed

        public int rd_pay;//last used Raw Day of paysheet
        public int rd_clover;//last used Raw Day of clover
        public int rd_clover_ad;//last used Raw Day of clover by ad

        public int lastRawRaidedDays;
        public int lastRaidedCount;

        public long veDate;
        public DateTime vipEndDate
        {
            get
            {
                return new DateTime(veDate);
            }
            set
            {
                veDate = value.Ticks;
            }
        }

        public GameItemData(bool resetData)
        {
            if (resetData)
                Reset();
        }

        public void Reset()
        {
            items = new List<Item>();
            mails = new List<MailItem>();
            shopRefreshCount = -1;

            rd_pay = -1;
            rd_clover = -1;
            rd_clover_ad = -1;
            lastRawRaidedDays = -1;
            lastRaidedCount = -1;

            paymentSc = new ShopCache();
            merchantSc = new ShopCache();
            vipSc = new ShopCache();
            normalSc = new ShopCache();
            matSc = new ShopCache();
            sumSc = new ShopCache();

            vipEndDate = DateTime.Now;
        }
    }

    [System.Serializable]
    public class GameData
    {
        /// <summary>
        /// 以下为道具数值记录
        /// </summary>
        public int exp;//checked
        public int playerLevel;//checked
        public int gold;//checked
        public int diamond;//checked

        public List<LevelItem> levels;
        public int lastRawPlayedDays;
        public int lastPlayedCount;

        public int buyPlayChanceCount;
        public int resetTalentCount;
        public int restockCount;

        public int talentPoint;//UnspentTalentPoints//checked
        public List<TalentItem> talentItems;//unspentTalentPoint is not a TalentItem//checked

        public List<ShipItem> shipItems;

        public List<string> missionsDoneMl;
        public int dailyRawDays;
        public int dlDone;
        public MissionItem missionMl;
        public MissionItem missionDl;

        /// <summary>
        /// 以下为历史记录
        /// </summary>
        public long icDate;
        public DateTime idleClaimedDate
        {
            get
            {
                return new DateTime(icDate);
            }
            set
            {
                icDate = value.Ticks;
            }
        }

        public string currentShipId;//checked
        public int rawLastPlayedDays_Clover;//每天占卜//checked
        public int rawLastPlayedDays_Salary;//每天工资//checked
        public int rawLastPlayedDays_freeDiamond;//checked
        public int rawLastPlayedDays_shark;//checked

        public FishingItem fishingItem;

        public GameData(bool resetData)
        {
            if (resetData)
                Reset();
        }

        public void Reset()
        {
            exp = 0;
            playerLevel = 1;
            gold = 0;
            diamond = 0;

            levels = new List<LevelItem>();
            lastRawPlayedDays = -1;
            lastPlayedCount = 0;

            buyPlayChanceCount = 0;
            resetTalentCount = 0;
            restockCount = 0;

            talentPoint = ConfigService.instance.talentConfig.initialPoints;
            talentItems = new List<TalentItem>();

            currentShipId = ConfigService.instance.combatConfig.playerParam.defaultShipId;
            shipItems = new List<ShipItem>();
            var ships = ConfigService.instance.factoryConfig.ships;
            foreach (var ship in ships)
            {
                var item = new ShipItem(ship.id);
                shipItems.Add(item);
                if (currentShipId == ship.id)
                    item.saveData.unlocked = true;
            }

            idleClaimedDate = DateTime.Now;
            rawLastPlayedDays_Clover = -1;
            rawLastPlayedDays_Salary = -1;
            rawLastPlayedDays_freeDiamond = -1;
            rawLastPlayedDays_shark = -1;

            fishingItem = new FishingItem();

            missionsDoneMl = new List<string>();
            dailyRawDays = -1;
            missionMl = null;
            missionDl = null;
        }
    }

    [System.Serializable]
    public class ShopCache
    {
        public List<ShopSlotCache> Slots;

        public ShopCache()
        {
            Clear();
        }
        public void Clear()
        {
            Slots = new List<ShopSlotCache>();
        }

        public void Clear(string id)
        {
            UnityEngine.Debug.Log("Clear " + id);
            foreach (var s in Slots)
            {
                UnityEngine.Debug.Log(s.id);
                if (s.id == id)
                {
                    UnityEngine.Debug.Log("remove");
                    Slots.Remove(s);
                    com.TextFormat.LogObj(Slots);
                    return;
                }
            }
        }

        public void Add(string id, int amount)
        {
            foreach (var s in Slots)
            {
                if (s.id == id)
                {
                    s.n += amount;//因为ShopSlotCache是class是个引用类型，所以这里直接加了，要注意一下
                    return;
                }
            }

            Slots.Add(new ShopSlotCache(id, amount));
        }
    }

    [System.Serializable]
    public class ShopSlotCache
    {
        public ShopSlotCache(string s, int a)
        {
            id = s;
            n = a;
        }

        public string id;
        public int n;
    }
}