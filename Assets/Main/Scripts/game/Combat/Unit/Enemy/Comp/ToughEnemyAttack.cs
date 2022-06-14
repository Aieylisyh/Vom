using UnityEngine;
using com;

namespace game
{
    public class ToughEnemyAttack : EnemyAttack
    {
        public ActivableSpecialModule asm;

        public float spiltFactor=1.4f;
        private bool _lastAttackIsRight;

        public override void Attack()
        {
            if (!self.IsAlive())
                return;

            if (_attackCounter == 0)
                return;

            if (!asm.rb.IsPlaying())
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

        protected override void LaunchAttack()
        {
            var trans = self.move.rotateAlignMove.trans;
            var attackXOffset = Random.Range(0, spiltFactor);
         
            if (_lastAttackIsRight)
            {
                attackXOffset = -attackXOffset;
            }
            _lastAttackIsRight = !_lastAttackIsRight;

            var dirPrimeReplace = new Vector3(attackXOffset, 1, 0);
            var dir = trans.forward * dirPrimeReplace.x + trans.up * dirPrimeReplace.y + trans.right * dirPrimeReplace.z;
            CreateTorDir(Vector3.zero, dir);
        }
    }
}