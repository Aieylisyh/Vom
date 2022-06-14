using UnityEngine;
using game;

namespace com
{
    public class SaveLoadService : MonoBehaviour
    {
        public static SaveLoadService instance;

        public enum StorageType
        {
            PlayerPrefs,
            EncryptedPrefs,
            EncryptedFile,
        }

        public StorageType storageType;
        public IStorageService storageService { get; private set; }

        private void Awake()
        {
            instance = this;
            switch (storageType)
            {
                case StorageType.EncryptedFile:
                    storageService = new EncryptedFileStorage();
                    break;

                case StorageType.EncryptedPrefs:
                    storageService = new EncryptedPrefsStorage();
                    break;

                case StorageType.PlayerPrefs:
                    storageService = new PlayerPrefsStorage();
                    break;
            }
        }

        private string _prefix = "";

        public void SetPrefix(string prefix1, string prefix2)
        {
            _prefix = StorageKey.SavePrefix + "_" + prefix1 + "_" + prefix2 + "_";
        }

        private string GetPrefixedKey(string key)
        {
            return _prefix + key;
        }

        private string GetSimpleKey(string prefix1, string prefix2)
        {
            return StorageKey.SavePrefix + "_" + prefix1 + "_" + prefix2;
        }

        private T LoadObject<T>(string key, T defaultValue)
        {
            string loaded = storageService.GetString(key, null);
            //Debug.Log("-----LoadObject " + key + " loaded:");
            //Debug.Log(loaded);
            return loaded == null ? defaultValue : JsonUtility.FromJson<T>(loaded);
        }

        public SettingsData LoadSettingsData(string slotId)
        {
            var key = GetSimpleKey(slotId, StorageKey.SettingsData);
            return LoadObject<SettingsData>(key, null);
        }

        public AccountData LoadAccountData(string slotId)
        {
            //Debug.LogWarning("LoadAccountData " + slotId);
            var key = GetSimpleKey(slotId, StorageKey.AccountData);
            return LoadObject<AccountData>(key, null);
        }

        public GameData LoadGameData(string slotId)
        {
            //Debug.LogWarning("LoadGameData " + slotId);
            var key = GetSimpleKey(slotId, StorageKey.GameData);
            return LoadObject<GameData>(key, null);
        }

        public GameItemData LoadGameItemData(string slotId)
        {
            //Debug.LogWarning("LoadGameItemData " + slotId);
            var key = GetSimpleKey(slotId, StorageKey.GameItemData);
            return LoadObject<GameItemData>(key, null);
        }

        public void SaveSettingsData(string slotId, SettingsData data)
        {
            var key = GetSimpleKey(slotId, StorageKey.SettingsData);
            storageService.SetString(key, JsonUtility.ToJson(data));
        }

        public void SaveAccountData(string slotId, AccountData data)
        {
            //Debug.LogWarning("SaveAccountData " + slotId);
            var key = GetSimpleKey(slotId, StorageKey.AccountData);
            storageService.SetString(key, JsonUtility.ToJson(data));
        }

        public void SaveGameData(string slotId, GameData data)
        {
            //Debug.LogWarning("SaveGameData " + slotId);
            var key = GetSimpleKey(slotId, StorageKey.GameData);
            storageService.SetString(key, JsonUtility.ToJson(data));
        }

        public void SaveGameItemData(string slotId, GameItemData data)
        {
            //Debug.LogWarning("SaveGameItemData " + slotId);
            var key = GetSimpleKey(slotId, StorageKey.GameItemData);
            //Debug.LogWarning(JsonUtility.ToJson(data));
            storageService.SetString(key, JsonUtility.ToJson(data));
        }
    }
}
