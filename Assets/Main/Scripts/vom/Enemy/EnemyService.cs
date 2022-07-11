using System.Collections.Generic;

namespace vom
{
    public class EnemyService
    {
        public static float GetSize(EnemySize size)
        {
            var cfg = GetCfg();
            switch (size )
            {
                case EnemySize.Tiny:
                    return cfg.sizes.tiny;

                case EnemySize.Small:
                    return cfg.sizes.small;

                case EnemySize.Mid:
                    return cfg.sizes.mid;

                case EnemySize.Big:
                    return cfg.sizes.big;

                case EnemySize.Huge:
                    return cfg.sizes.huge;
            }

            return 0;
        }

        public static EnemyConfig GetCfg()
        {
            return ConfigSystem.instance.enemyConfig;
        }

        public static EnemyPrototype GetPrototype(string id)
        {
            foreach (var s in GetCfg().enemies)
            {
                if (s.id == id)
                    return s;
            }

            return null;
        }
    }
}
