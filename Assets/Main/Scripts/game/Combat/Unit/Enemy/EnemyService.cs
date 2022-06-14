using UnityEngine;

namespace game
{
    public class EnemyService : MonoBehaviour
    {
        public static EnemyService instance;

        private void Awake()
        {
            instance = this;
        }

        public EnemyPrototype GetPrototype(string id)
        {
            foreach (var i in ConfigService.instance.enemyConfig.list)
            {
                if (i.id == id)
                {
                    return i;
                }
            }
            return null;
        }

        public EnemyPrototype GetPrototype(Unit self)
        {
            Enemy e = self as Enemy;
            return GetPrototype(e.id);
        }
    }
}
