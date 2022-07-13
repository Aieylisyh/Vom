using UnityEngine;

namespace vom
{
    public class EnemySpawnBehaviour : PropsSpawnBehaviour
    {
        public EnemyPrototype enemy;

        protected override void Spawn()
        {
           var e= Instantiate(enemy.prefab, transform.position, Quaternion.identity, transform.parent);
            e.move.rotatePart.localEulerAngles = transform.localEulerAngles;
        }
    }
}