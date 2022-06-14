using UnityEngine;
using com;
namespace game
{
    public class ProjectileCollision : UnitCollision
    {
        public bool waterCollision = true;
        public string waterCollisionPrefabId = "out water";
        public bool waterCollisionDie = false;

        private bool _collided;

        public override void ResetState()
        {
            base.ResetState();
            _collided = false;
        }

        protected override void OnTriggerEntered(Collider other)
        {
            if (waterCollision && other.CompareTag("waterPlane"))
            {
                OnWaterCollided();
            }

            base.OnTriggerEntered(other);
        }

        private void OnWaterCollided()
        {
            if (!string.IsNullOrEmpty(waterCollisionPrefabId))
            {
                var go = PoolingService.instance.GetInstance(waterCollisionPrefabId);
                go.transform.position = transform.position;
            }

            if (waterCollisionDie)
            {
                self.death.Die(false);
            }
            else
            {
                Projectile p = self as Projectile;
                p.DetachBubble();
            }
        }

        protected override void OnCollided(Unit target)
        {
            base.OnCollided(target);
            if (self.death != null && self.death.isDead)
                return;

            if (target is Enemy && !target.IsAlive())
                return;

            if (_collided)
                return;

            _collided = true;
            if (self.health != null)
            {
                //Debug.Log("projectile OnCollided ");
                self.health.OnReceiveDamage();
            }
            else if (self.death != null)
            {
                self.death.Die(false);
            }
            else
            {
                self.Recycle();
            }
        }
    }
}
