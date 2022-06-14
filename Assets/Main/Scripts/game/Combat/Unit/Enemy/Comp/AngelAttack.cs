using UnityEngine;
using com;

namespace game
{
    public class AngelAttack : EnemyAttack
    {
        public float offsetSpawn = 1;
        public float offset1 = 1;
        public float offset2 = 0;
        public float offset3 = -1;

        public override void Attack()
        {
            var dir = GetRelativeDirection(dirPrime);
            CreateGhost(offset1, dir);
            CreateGhost(offset2, dir);
            CreateGhost(offset3, dir);

            AttackFeedback();
        }

        private void CreateGhost(float directAimOffset, Vector3 primeDir)
        {
            string prefabId = projectileId;
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.directAimOffset = directAimOffset;
            tor.Init(muzzlePos.position + Vector3.right * Random.Range(-offsetSpawn, offsetSpawn), primeDir);
            var enemyLevel = (self as Enemy).enemyLevel;
            tor.SetGhostDamage(enemyLevel);
        }
    }
}