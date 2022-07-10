using UnityEngine;
using com;

namespace vom
{
    public interface IRuntimeDataCache<T>
    {
        void Create();
        void Save();
        void Load();
        void SetId(string id);
        T cache { get; set; }
    }

    public class SettingsDataCache : IRuntimeDataCache<SettingsData>
    {
        private SettingsData _cache;
        private string _saveid;

        SettingsData IRuntimeDataCache<SettingsData>.cache
        {
            get => _cache;
            set => _cache = value;
        }
        void IRuntimeDataCache<SettingsData>.Create()
        {
            _cache = new SettingsData();
        }
        void IRuntimeDataCache<SettingsData>.Save()
        {
         //   SaveLoadService.instance.SaveSettingsData(_saveid, _cache);
        }
        void IRuntimeDataCache<SettingsData>.Load()
        {
           // _cache = SaveLoadService.instance.LoadSettingsData(_saveid);
        }
        void IRuntimeDataCache<SettingsData>.SetId(string id)
        {
            _saveid = id;
        }
    }

    public class AccountDataCache : IRuntimeDataCache<AccountData>
    {
        private AccountData _cache;
        private string _saveid;

         AccountData IRuntimeDataCache<AccountData>.cache {
            get =>  _cache;
            set => _cache = value;
        }
        void IRuntimeDataCache<AccountData>.Create()
        {
            _cache = new AccountData();
        }
        void IRuntimeDataCache<AccountData>.Save()
        {
        //    SaveLoadService.instance.SaveAccountData(_saveid, _cache);
        }
        void IRuntimeDataCache<AccountData>.Load()
        {
           // _cache = SaveLoadService.instance.LoadAccountData(_saveid);
        }
        void IRuntimeDataCache<AccountData>.SetId(string id)
        {
            _saveid = id;
        }
    }

    public class GameDataCache : IRuntimeDataCache<GameData>
    {
        private GameData _cache;
        private string _saveid;

        GameData IRuntimeDataCache<GameData>.cache
        {
            get => _cache;
            set => _cache = value;
        }
        void IRuntimeDataCache<GameData>.Create()
        {
            _cache = new GameData(true);
        }
        void IRuntimeDataCache<GameData>.Save()
        {
          //  SaveLoadService.instance.SaveGameData(_saveid, _cache);
        }
        void IRuntimeDataCache<GameData>.Load()
        {
          //  _cache = SaveLoadService.instance.LoadGameData(_saveid);
        }
        void IRuntimeDataCache<GameData>.SetId(string id)
        {
            _saveid = id;
        }
    }

    public class GameItemDataCache : IRuntimeDataCache<GameItemData>
    {
        private GameItemData _cache;
        private string _saveid;

        GameItemData IRuntimeDataCache<GameItemData>.cache
        {
            get => _cache;
            set => _cache = value;
        }
        void IRuntimeDataCache<GameItemData>.Create()
        {
            _cache = new GameItemData(true);
        }
        void IRuntimeDataCache<GameItemData>.Save()
        {
          //  SaveLoadService.instance.SaveGameItemData(_saveid, _cache);
        }
        void IRuntimeDataCache<GameItemData>.Load()
        {
       //     _cache = SaveLoadService.instance.LoadGameItemData(_saveid);
        }
        void IRuntimeDataCache<GameItemData>.SetId(string id)
        {
            _saveid = id;
        }
    }
}