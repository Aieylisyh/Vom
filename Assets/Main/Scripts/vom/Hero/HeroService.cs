namespace vom
{
    public class HeroService
    {
        public static HeroPrototype GetPrototype(string id)
        {
            foreach (var s in ConfigSystem.instance.heroConfig.heroes)
            {
                if (s.id == id)
                    return s;
            }

            return null;
        }
    }
}