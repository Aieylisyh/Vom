using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class TalentPrototype : ScriptableObject
    {
        public string id;
        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }
        //public string subDesc { get { return id + "_subDesc"; } }
        public List<string> baseTalents = new List<string>();
        public bool hasIntValue;
        public string stringValue;
        public Sprite sp;
        public List<TalentLevel> levels;

        public TalentCategory category;
        public int uiIndex;

        public int GetMaxLevel()
        {
            return levels.Count;
        }

        public TalentLevel GetLevel(int level)
        {
            return levels[level - 1];
        }

        public int GetIntValue(int level)
        {
            if (level == 0)
            {
                return 0;
            }

            return GetLevel(level).intValue;
        }

        public string GetStringValue()
        {
            return stringValue;
        }

        public string GetLocalizedTitle()
        {
            if (string.IsNullOrEmpty(GetStringValue()))
            {
                return com.LocalizationService.instance.GetLocalizedText(title);
            }
            else
            {
                var tp = com.LocalizationService.instance.GetLocalizedText(GetStringValue());
                return com.LocalizationService.instance.GetLocalizedTextFormatted(title, tp);
            }
        }
    }

    [System.Serializable]
    public struct TalentLevel
    {
        public int intValue;
    }

    public enum TalentCategory
    {
        Hull,
        Bomb,
        Tor,
    }
}