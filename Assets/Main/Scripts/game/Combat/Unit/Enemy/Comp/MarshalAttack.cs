using UnityEngine;
using com;

namespace game
{
    public class MarshalAttack : EnemyAttack
    {
        public float offset1 = 1;
        public float offset2 = 2;
        public float offset3 = 3;

        protected override void CreateTorDir(Vector3 offset, Vector3 primeDir)
        {
            CreateMarshalTor(offset, primeDir, offset1);
            CreateMarshalTor(offset, primeDir, offset2);
            CreateMarshalTor(offset, primeDir, offset3);
        }

        private void CreateMarshalTor(Vector3 offset, Vector3 primeDir, float directAimOffset)
        {
            string prefabId = projectileId;
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.directAimOffset = directAimOffset;
            tor.Init(muzzlePos.position + offset, primeDir);
            tor.SetDamage(_damage);
        }
    }
}