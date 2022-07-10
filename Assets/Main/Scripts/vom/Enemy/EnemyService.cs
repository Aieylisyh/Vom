using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EnemyService
    {
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
