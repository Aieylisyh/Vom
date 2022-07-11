namespace vom
{
    public class DailyPerkService
    {
        public static DailyPerkPrototype GetPrototype(string id)
        {
            foreach (var s in ConfigSystem.instance.dailyLevelConfig.perks)
            {
                if (s.id == id)
                    return s;
            }

            return null;
        }

        public static DailyLevelConfig GetCfg()
        {
            return ConfigSystem.instance.dailyLevelConfig;
        }

        public static int GetLevelMaxExp(int level)
        {
            return GetCfg().experienceRequirement.GetIntValue(level);
        }

        public static void SetDataByTotalExp(int totalExp, ref DailyLevelData data)
        {
            //UnityEngine.Debug.Log("SetDataByTotalExp" + totalExp);
            int expMax = 0;
            for (var lv = 1; lv <= GetCfg().maxLevel; lv++)
            {
                expMax = GetLevelMaxExp(lv);
                if (totalExp >= expMax)
                    totalExp -= expMax;
                else
                {
                    data.level = lv;
                    data.exp = totalExp;
                    return;
                }
            }

            data.level = GetCfg().maxLevel;
            data.exp = expMax;
        }
    }
}