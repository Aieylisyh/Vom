using UnityEngine;

namespace vom
{
    public class EnemySpawnBehaviour : PropsSpawnBehaviour
    {
        public EnemyPrototype enemy;

        protected override void Spawn()
        {
            Instantiate(enemy.prefab, transform.position, transform.rotation, transform.parent);
        }
    }
}