using UnityEngine;

namespace vom
{
    public class EnemySpawnBehaviour : PropsSpawnBehaviour
    {
        public EnemyPrototype enemy;

        protected override void Spawn()
        {
            var ene = Instantiate(enemy.prefab, transform.position, transform.rotation, transform.parent);
            ene.AssignAttibution(enemy);
        }
    }
}