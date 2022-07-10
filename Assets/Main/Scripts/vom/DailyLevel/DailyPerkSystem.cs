using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class DailyPerkSystem : MonoBehaviour
    {
        public static DailyPerkSystem instance { get; private set; }

        private DailyLevelData _data = new DailyLevelData();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            InitData();
        }

        void InitData()
        {
            _data = new DailyLevelData();
            _data.level = 1;
            _data.exp = 0;
            _data.perks = new List<string>();
        }

        public int expMax { get { return DailyPerkService.GetLevelMaxExp(_data.level); } }

        public int level { get { return _data.level; } }

        public int exp { get { return _data.exp; } }

        public void SyncExp()
        {
            int currentLevel = level;
            DailyPerkService.SetDataByTotalExp(InventorySystem.instance.ExpCount, ref _data);

            var levelup = level - currentLevel;
            OnLevelUp(levelup);

            MainHudBehaviour.instance.SyncExp();
        }

        void OnLevelUp(int times)
        {
            if (times > 0)
            {
                Debug.Log("OnLevelUp " + times);
                //TODO +perk
            }
        }
    }
}