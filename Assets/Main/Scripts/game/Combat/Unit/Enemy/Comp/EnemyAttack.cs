using UnityEngine;
using com;

namespace game
{
    public class EnemyAttack : UnitAttack
    {
        public AttackAction postAction;
        public string projectileId;
        public Vector3 dirPrime = Vector3.up;
        protected int _damage;

        public Transform muzzlePos;
        public bool hasMuzzleEffect = true;
        public string muzzlePrefabId = "basic muzzle";
        public int attackCount = -1;//<0 = infinity
        protected int _attackCounter;

        protected Vector3 GetRelativeDirection(Vector3 baseDir)
        {
            var trans = self.move.rotateAlignMove.trans;
            return  trans.forward * baseDir.x + trans.up * baseDir.y + trans.right * baseDir.z;
        }

        public void Init(int damage)
        {
            _damage = damage;
        }

        protected override void Tick()
        {

        }

        public override void Attack()
        {
            if (!self.IsAlive())
                return;

            if (_attackCounter == 0)
                return;

            _attackCounter--;

            LaunchAttack();
            AttackFeedback();
            switch (postAction)
            {
                case AttackAction.DieSilent:
                    self.death.Die(true);
                    break;
                case AttackAction.DieUnSilent:
                    self.death.Die(false);
                    break;
            }
        }

        protected virtual void LaunchAttack()
        {
            var dir = GetRelativeDirection(dirPrime);
            CreateTorDir(Vector3.zero, dir);
        }

        protected virtual void AttackFeedback()
        {
            SoundService.instance.Play(attackSound);

            if (hasMuzzleEffect)
            {
                var go = PoolingService.instance.GetInstance(muzzlePrefabId);
                go.transform.position = muzzlePos.position;
            }
        }

        protected virtual void CreateTorDir(Vector3 offset, Vector3 primeDir)
        {
            string prefabId = projectileId;
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.Init(muzzlePos.position + offset, primeDir);
            tor.SetDamage(_damage);
        }

        public override void ResetState()
        {
            _attackCounter = attackCount;
            base.ResetState();
        }

        public enum AttackAction
        {
            None,
            DieSilent,
            DieUnSilent,
        }
    }
}