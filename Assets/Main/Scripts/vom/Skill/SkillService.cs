namespace vom
{
    public class SkillService
    {
        public static SkillPrototype GetPrototype(string id)
        {
            foreach (var s in ConfigSystem.instance.skillConfig.skills)
            {
                if (s.id == id)
                    return s;
            }

            return null;
        }
    }
}