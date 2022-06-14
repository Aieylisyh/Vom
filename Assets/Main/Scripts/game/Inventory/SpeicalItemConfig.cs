using System.Collections.Generic;
using UnityEngine;
using com;
using System;

namespace game
{
    [CreateAssetMenu]
    public class SpeicalItemConfig : ScriptableObject
    {
        public Formula salaryGoldByPlayerLevel;

        public List<CloverDayData> WeekdayData;//FortuneTelling

        public string noteIdPrefix = "Note_";
        public int noteIdMax;

        public string calmSoundId = "Clover c";
        public string pleasantSoundId = "Clover p";
        public string wonderfulSoundId = "Clover w";

        [Serializable]
        public struct CloverDayData
        {
            public CloverLuckyData calm;
            public CloverLuckyData pleasant;
            public CloverLuckyData wonderful;
        }

        [Serializable]
        public struct CloverLuckyData
        {
            public string dayTitle;
            public string dayContent;
            public List<Item> rewards;
        }
    }
}