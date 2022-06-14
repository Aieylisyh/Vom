using UnityEngine;
using com;

namespace game
{
    public class LaserAttack : EnemyAttack
    {
        protected override void LaunchAttack()
        {
            CreateLL();
        }

        private void CreateLL()
        {
            string prefabId = projectileId;
            var go = PoolingService.instance.GetInstance(prefabId);
            go.transform.position = muzzlePos.position;

            Enemy enemy = go.GetComponent<Enemy>();
            enemy.InitSpawned(prefabId);
        }
    }
}