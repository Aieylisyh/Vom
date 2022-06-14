using System.Collections.Generic;
using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    public class MissionPrototype : ScriptableObject
    {
        public string id;

        public List<Item> reward;

        public Content content;

        public enum MissionType
        {
            None,
            PassLevel,//level id
            KillEnemy,//
            PassWave,//
            UseItem,//id
            EnterCombat,
            UpgradeShip,//to level
            CheckTown,
            Ad,
        }

        [System.Serializable]
        public struct Content
        {
            public int quota;
            public int paramInt1;
            public string paramString1;
            public MissionType type;
        }

        public string GetMissionKey()
        {
            return "Ms_" + content.type.ToString();
        }

        public string GetMissionText()
        {
            var key = GetMissionKey();

            string res = "";
            switch (content.type)
            {
                case MissionType.None:
                    break;

                case MissionType.PassWave:
                    res = LocalizationService.instance.GetLocalizedTextFormatted(key, content.paramInt1);
                    break;

                case MissionType.KillEnemy:
                    res = LocalizationService.instance.GetLocalizedTextFormatted(key, content.paramInt1);
                    break;

                case MissionType.PassLevel:
                    var levelnameKey = LevelService.instance.GetPrototype(content.paramString1).title;
                    var levelname = LocalizationService.instance.GetLocalizedText(levelnameKey);
                    res = LocalizationService.instance.GetLocalizedTextFormatted(key, levelname);
                    break;

                case MissionType.UseItem:
                    var itemNameKey = ItemService.instance.GetPrototype(content.paramString1).title;
                    var itemName = LocalizationService.instance.GetLocalizedText(itemNameKey);
                    res = LocalizationService.instance.GetLocalizedTextFormatted(key, itemName);
                    break;

                case MissionType.Ad:
                    res = LocalizationService.instance.GetLocalizedText(key);
                    break;

                case MissionType.EnterCombat:
                    res = LocalizationService.instance.GetLocalizedText(key);
                    break;

                case MissionType.CheckTown:
                    res = LocalizationService.instance.GetLocalizedText(key);
                    break;

                case MissionType.UpgradeShip:
                    var shipName = LocalizationService.instance.GetLocalizedText(content.paramString1);
                    res = LocalizationService.instance.GetLocalizedTextFormatted(key, shipName, content.paramInt1);
                    break;
            }

            return res;
        }
    }
}