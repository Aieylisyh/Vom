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
    }
}