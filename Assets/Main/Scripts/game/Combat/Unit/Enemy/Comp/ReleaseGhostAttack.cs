using UnityEngine;
using com;

namespace game
{
    public class ReleaseGhostAttack : EnemyAttack
    {
        public bool randomizeGhostPosition;
        public float offset = 1.8f;

        protected override void LaunchAttack()
        {
            CreateGhost();
        }

        private void CreateGhost()
        {
            string prefabId = projectileId;
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.directAimOffset = 0;
            var pos = transform.position;
            if (randomizeGhostPosition)
            {
                var lfp = ConfigService.instance.combatConfig.levelFieldParam;
                pos.x = Random.Range(lfp.boundLeft + offset, lfp.boundRight - offset);
            }
            else
            {
                pos = muzzlePos.position;
            }
            tor.Init(pos, Vector3.up);

            var enemyLevel = (self as Enemy).enemyLevel;
            tor.SetGhostDamage(enemyLevel);
        }
    }
}