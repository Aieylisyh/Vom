using UnityEngine;
using com;

namespace game
{
    public class LaserLauncherAttack : EnemyAttack
    {
        public LineRendererLaser lrl;
        public float laserDuration = 1;
        private float _laserTimer;
        private float _dmgPerTick;
        public float range = 1.4f;

        public override void ResetState()
        {
            lrl.Stop();
            base.ResetState();
        }

        protected override void Tick()
        {
            if (!self.IsAlive())
                return;

            if (_laserTimer <= 0)
                return;

            _laserTimer -= TickTime;
            var pShip = CombatService.instance.playerShip;
            var deltaX = pShip.move.transform.position.x - lrl.transform.position.x;
            if (Mathf.Abs(deltaX) < range)
                pShip.health.OnReceiveDamage(new Damage(self, (int)_dmgPerTick, DamageType.Laser, true));
        }

        protected override void LaunchAttack()
        {
            _laserTimer = laserDuration;
            _dmgPerTick = (float)_damage * TickTime;
        }

        protected override void AttackFeedback()
        {
            base.AttackFeedback();
            lrl.Play();
        }
    }
}