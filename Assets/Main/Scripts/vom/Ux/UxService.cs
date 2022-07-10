using com;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace vom
{
    public class UxService : MonoBehaviour
    {
        public static UxService instance { get; private set; }

        public const int EventRoutineDayCount = 3;

        public const int ShopRefreshHours = 8;

        public List<IRuntimeDataCache<AccountData>> accountsDataCache { get; private set; }
        public IRuntimeDataCache<AccountData> accountDataCache
        {
            get
            {
                return accountsDataCache[accountId - 1];
            }
            private set
            {
                accountsDataCache[accountId - 1] = value;
            }
        }

        public List<IRuntimeDataCache<GameData>> gamesDataCache { get; private set; }
        public IRuntimeDataCache<GameData> gameDataCache
        {
            get
            {
                return gamesDataCache[accountId - 1];
            }
            private set
            {
                gamesDataCache[accountId - 1] = value;
            }
        }

        public IRuntimeDataCache<GameItemData> gameItemDataCache { get; private set; }
        public IRuntimeDataCache<SettingsData> settingsDataCache { get; private set; }

        public int accountId { get; private set; }//1 2 3

        private bool _toSaveGameData = false;
        private bool _toSaveGameItemData = false;
        private bool _toSaveAccountData = false;
        private bool _toSaveSettingsData = false;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (_toSaveGameItemData)
            {
                _toSaveGameItemData = false;
                gameItemDataCache.Save();
            }
            if (_toSaveGameData)
            {
                _toSaveGameData = false;
                gameDataCache.Save();
            }
            if (_toSaveAccountData)
            {
                _toSaveAccountData = false;
                accountDataCache.Save();
            }
            if (_toSaveSettingsData)
            {
                _toSaveSettingsData = false;
                settingsDataCache.Save();
            }
        }

        private string GetSlotId(int index)
        {
            return "s" + index;
        }

        public void StartLogin()
        {
            Debug.Log("StartLogin");
            accountsDataCache = new List<IRuntimeDataCache<AccountData>>();
            gamesDataCache = new List<IRuntimeDataCache<GameData>>();

            for (var i = 0; i < 3; i++)
            {
                accountsDataCache.Add(new AccountDataCache());
                gamesDataCache.Add(new GameDataCache());
            }

            for (var i = 0; i < 3; i++)
            {
                var tpSlotId = GetSlotId(i + 1);
                InitDataCache(accountsDataCache[i], tpSlotId);
                InitDataCache(gamesDataCache[i], tpSlotId);
            }

            LoadSettingsAndStartMusic();

            // then show account view;
            //WindowService.instance.ShowLogin();
        }

        void LoadSettingsAndStartMusic()
        {
            Debug.Log("LoadSettingsAndStartMusic");
            settingsDataCache = new SettingsDataCache();
            InitDataCache(settingsDataCache, "u", true);

            MusicService.instance.IsEnabled = settingsDataCache.cache.musicOn;
            LocalizationService.instance.currentLanguage = settingsDataCache.cache.language;
            LocalizeLabel.OnLanguageChange();
        }

        public void SelectAccount(int index)
        {
            accountId = index;
            SyncSelectedDataCache();
            // StartGameService.instance.LoginFinishStartGame();
        }

        public void ClearAccount(int pAccountId)
        {
            Debug.LogWarning("ClearAccount " + pAccountId);
            var tpSlotId = GetSlotId(pAccountId);

            //  SaveLoadService.instance.SaveAccountData(tpSlotId, null);
            //  SaveLoadService.instance.SaveGameData(tpSlotId, null);
            //  SaveLoadService.instance.SaveGameItemData(tpSlotId, null);

            accountsDataCache[pAccountId - 1] = new AccountDataCache();
            gamesDataCache[pAccountId - 1] = new GameDataCache();
            InitDataCache(accountsDataCache[pAccountId - 1], tpSlotId);
            InitDataCache(gamesDataCache[pAccountId - 1], tpSlotId);
        }

        public void ReloadAccounts()
        {
            for (var i = 0; i < 3; i++)
            {
                var tpSlotId = GetSlotId(i + 1);
                InitDataCache(accountsDataCache[i], tpSlotId);
                InitDataCache(gamesDataCache[i], tpSlotId);
            }
        }

        private void SyncSelectedDataCache()
        {
            Debug.Log("!SyncSelectedDataCache " + accountId);
            var id = GetSlotId(accountId);
            if (accountDataCache.cache == null)
            {
                accountDataCache.Create();
                accountDataCache.cache.slotId = id;
                accountDataCache.cache.firstLaunchDate = System.DateTime.Now;
                accountDataCache = accountDataCache;
            }

            accountDataCache.cache.lastLaunchDate = System.DateTime.Now;
            SaveAccountData();

            if (gameDataCache.cache == null)
            {
                gameDataCache.Create();
                gameDataCache = gameDataCache;
                SaveGameData();
            }

            gameItemDataCache = new GameItemDataCache();
            InitDataCache(gameItemDataCache, id, true);
        }

        private void InitDataCache<T>(IRuntimeDataCache<T> c, string id, bool create = false)
        {
            c.SetId(id);
            c.Load();

            if (c.cache == null && create)
            {
                c.Create();
                c.Save();
            }
        }

        public void SaveAccountData()
        {
            _toSaveAccountData = true;
        }

        public void SaveGameData()
        {
            _toSaveGameData = true;
        }

        public void SaveGameItemData()
        {
            _toSaveGameItemData = true;
        }

        public void SaveSettingsData()
        {
            _toSaveSettingsData = true;
        }

        public System.TimeSpan GetPlayedTimeSpan()
        {
            return now - GetFirstLaunchDate();
        }

        public System.DateTime GetFirstLaunchDate()
        {
            return accountDataCache.cache.firstLaunchDate;
        }

        public System.DateTime GetLastLaunchDate()
        {
            return accountDataCache.cache.lastLaunchDate;
        }

        public System.DateTime GetRawFirstLaunchDate()//FloorToDay
        {
            var t = GetFirstLaunchDate();
            return new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0);
        }

        public int GetPlayedHours()
        {
            var s = GetPlayedTimeSpan();
            return (int)s.TotalHours;//s.Hours is 0 after 1 day
        }

        public int GetShopRefreshCount()
        {
            int h = GetPlayedHours();
            return h / ShopRefreshHours;
        }

        public int GetPlayedDays()
        {
            var s = GetPlayedTimeSpan();
            return (int)s.TotalDays;
        }

        public System.TimeSpan GetRawPlayedTimeSpan()//From Zero o Clock
        {
            return now - GetRawFirstLaunchDate();
        }

        public int GetRawPlayedDays()//From Zero o Clock
        {
            var s = GetRawPlayedTimeSpan();
            return (int)s.TotalDays;
            //return s.Days;
        }

        public static System.DateTime now
        {
            get { return System.DateTime.Now; }
        }

        public int GetEventCount()
        {
            int rpd = GetRawPlayedDays();
            return rpd / EventRoutineDayCount;
        }

        public int GetEventIndex()
        {
            int eventCount = GetEventCount();
            return eventCount % 4;
        }

        public string GetEventTokenId()
        {
            return "Token" + (1 + GetEventIndex());
        }

        public System.TimeSpan GetEventTimer(int count)
        {
            var rawFirstLaunchDate = GetRawFirstLaunchDate();
            int daysPassedTotalWholeEvents = count * EventRoutineDayCount;
            var eventRestDays = rawFirstLaunchDate.AddDays(daysPassedTotalWholeEvents);
            return eventRestDays - now;
        }

        public System.TimeSpan GetShopRefreshTimer()
        {
            var stp = GetPlayedTimeSpan();
            // Debug.Log("PlayedTimeSpan " + stp.ToString());
            float countPlus = (float)stp.TotalHours / (float)ShopRefreshHours;
            //Debug.Log("countPlus " + countPlus);
            int nextPassedHours = Mathf.CeilToInt(countPlus) * ShopRefreshHours;
            // Debug.Log("nextPassedHours " + nextPassedHours);
            var nextCeilDate = GetFirstLaunchDate().AddHours(nextPassedHours);
            //Debug.Log("nextCeilDate " + nextCeilDate.ToString());
            return nextCeilDate - now;
        }

        public bool CheckShopCache()
        {
            //Debug.Log("CheckShopCache ");
            bool refreshed = false;
            if (GetShopRefreshCount() != gameItemDataCache.cache.shopRefreshCount)
            {
                //Debug.Log("shops refreshed！");
                //paymentShopCache 不需要更新
                gameItemDataCache.cache.merchantSc = new ShopCache();
                gameItemDataCache.cache.vipSc = new ShopCache();
                gameItemDataCache.cache.normalSc = new ShopCache();
                gameItemDataCache.cache.matSc = new ShopCache();
                gameItemDataCache.cache.shopRefreshCount = GetShopRefreshCount();
                refreshed = true;
            }

            return refreshed;
        }

        //  public void UpdateShopCache(CommodityPopup.CommodityPopupData data, string id, int amount)
        //  {
        //      //Debug.Log(shopCategory + " shop UpdateShopCache " + id + " " + amount);
        //      gameItemDataCache.cache.sumSc.Add(id, amount);
        //
        //      var shopCache = ShopService.instance.GetShopCache(data.shopCategory);
        //      shopCache.Add(id, amount);
        //
        //      if (data.commodity.disablePostBuyRefresh)
        //      {
        //          //Debug.Log("disablePostBuyRefresh " + id);
        //          var cfg = ShopService.instance.GetShopCfg(data.shopCategory);
        //          var itemsOfSlot = cfg.commodityCfgs[data.indexOfList];
        //          foreach (var commoditySlotData in itemsOfSlot.items)
        //          {
        //              if (commoditySlotData.disablePostBuyRefresh)
        //              {
        //                  var cmd = commoditySlotData.commodity;
        //                  shopCache.Add(cmd.id, 1);
        //                  //Debug.Log("shopCache Add " + cmd.id);
        //              }
        //          }
        //      }
        //
        //      //       WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
        //      SaveGameItemData();
        //  }

        public void RemoveShopCache(ShopService.ShopCategory shopCategory, string id)
        {
            var shopCache = ShopService.instance.GetShopCache(shopCategory);
            gameItemDataCache.cache.sumSc.Clear(id);
            shopCache.Clear(id);

            //  WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
            SaveGameItemData();
        }

        public System.TimeSpan GetRestTimeVip()
        {
            var delta = gameItemDataCache.cache.vipEndDate - now;
            //Debug.Log("GetRestTimeVip");
            //Debug.Log(gameItemDataCache.cache.vipEndDate);
            if (delta.Ticks > 0)
                return delta;

            return System.TimeSpan.FromTicks(0);
        }

        public void AddRestTimeVip(int d, int h, int m, int s)
        {
            System.TimeSpan deltaD = System.TimeSpan.FromDays(d);
            System.TimeSpan deltaH = System.TimeSpan.FromHours(h);
            System.TimeSpan deltaM = System.TimeSpan.FromMinutes(m);
            System.TimeSpan deltaS = System.TimeSpan.FromSeconds(s);
            gameItemDataCache.cache.vipEndDate = now + deltaD + deltaH + deltaM + deltaS;
            SaveGameItemData();
        }

        //this function is used when in a transaction, check the price afforability
        public int GetItemAmount(string id)
        {
            if (id == "Gold")
                return gameDataCache.cache.gold;
            if (id == "Free")
                return 1;
            if (id == "Ad")
                return AdService.instance.CanPlayAd(true) ? 1 : 0;
            if (id == "PlayerLevel")
                return gameDataCache.cache.playerLevel;
            if (id == "Exp")
                return gameDataCache.cache.exp;
            if (id == "Diamond")
                return gameDataCache.cache.diamond;
            if (id == "TalentPoint")
                return gameDataCache.cache.talentPoint;

            var items = GetAllItems();
            foreach (var i in items)
            {
                if (id == i.id)
                    return i.n;
            }
            return 0;
        }

        //this function is used when change the amount of any item, including all price types
        public void AddItem(ItemData item)
        {
            AddItem(item.id, item.n);
        }

        public void AddItem(string id, int amountDelta)
        {
            bool finished = false;
            bool changedHudItem = false;
            var proto = ItemService.GetPrototype(id);

            if (id == "Free")
            {
                finished = true;
                changedHudItem = false;
            }
            else if (id == "Ad")
            {
                Debug.LogWarning("AddItem id==ad, Ad is not played this way");
                finished = true;
                changedHudItem = false;
            }
            else if (id == "Gold")
            {
                gameDataCache.cache.gold += amountDelta;
                finished = true;
                changedHudItem = true;
            }
            else if (id == "PlayerLevel")
            {
                gameDataCache.cache.playerLevel += amountDelta;
                finished = true;
                changedHudItem = true;
            }
            else if (id == "Exp")
            {
                //AddExp(amountDelta);
                finished = true;
                changedHudItem = true;
            }
            else if (id == "Diamond")
            {
                gameDataCache.cache.diamond += amountDelta;
                finished = true;
                changedHudItem = true;
            }
            else if (id == "TalentPoint")
            {
                gameDataCache.cache.talentPoint += amountDelta;
                finished = true;
                changedHudItem = true;
            }

            // var parsedRes = ConfigService.instance.itemsConfig.ParseComplexItem(id, amountDelta);
            // if (parsedRes != null && parsedRes.Count > 0)
            // {
            //     foreach (var i in parsedRes)
            //     {
            //         //Debug.Log("res " + i.id + " " + i.n);
            //         AddItem(i.id, i.n);
            //     }
            //     return;
            // }

            var allItems = GetAllItems();
            if (!finished)
            {
                foreach (var i in allItems)
                {
                    if (id == i.id)
                    {
                        i.n = i.n + amountDelta;
                        finished = true;
                        if (i.n == 0)
                        {
                            allItems.Remove(i);
                        }
                        break;
                    }
                }
            }

            if (!finished)
            {
                if (proto != null)
                {
                    allItems.Add(new ItemData(amountDelta, id));
                }
                else
                {
                    Debug.LogError(id + " has no item prototype!");
                }
                finished = true;
            }

            //     if (changedHudItem)   MainHudBehaviour.instance.Refresh();

            SaveGameItemData();
        }

        public void SortItems()
        {
            var newList = new List<ItemData>();
            var cfg = ConfigSystem.instance.itemsConfig;
            var allItems = GetAllItems();

            foreach (var c in cfg.items)
            {
                foreach (var item in allItems)
                {
                    if (item != null && c != null && c.id == item.id)
                    {
                        if (item.n > 0)
                        {
                            newList.Add(new ItemData(item.n, item.id));
                        }
                        break;
                    }
                }
            }

            gameItemDataCache.cache.items = newList;
        }

        public void LogItems()
        {
            Debug.Log("--LogItems--");
            var items = GetAllItems();
            foreach (var item in items)
            {
                Debug.Log(item.id + " " + item.n);
            }
        }

        public List<ItemData> GetAllItems()
        {
            return gameItemDataCache.cache.items;
        }

        public List<ItemData> GetInventoryItems()
        {
            List<ItemData> res = new List<ItemData>();
            foreach (var item in gameItemDataCache.cache.items)
            {
                if (!ItemService.GetPrototype(item.id).invHide)
                    res.Add(item);
            }

            return res;
        }

        public static int GetDayIndexOfWeek()
        {
            var now = DateTime.Now;
            var dayOfWeek = now.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
            }

            return 0;
        }

        public void CheckRaidCount()
        {
            var lrpd = gameItemDataCache.cache.lastRawRaidedDays;
            var rpd = GetRawPlayedDays();
            if (lrpd != rpd)
            {
                gameItemDataCache.cache.lastRaidedCount = 0;
                gameItemDataCache.cache.lastRawRaidedDays = rpd;
                SaveGameData();
            }
        }
    }
}