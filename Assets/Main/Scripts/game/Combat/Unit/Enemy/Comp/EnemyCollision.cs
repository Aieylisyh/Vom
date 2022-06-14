using UnityEngine;

namespace game
{
    public class EnemyCollision : UnitCollision
    {
        protected override void OnCollided(Unit target)
        {
            base.OnCollided(target);
            //Debug.Log("EnemyCollision OnCollided");
            if (!self.IsAlive())
                return;

            if (self.health != null)
            {
                //Debug.Log("Enemy OnCollided by " + target.gameObject.name);
                //Debug.Log(target.attack.dmg.value);
                self.health.OnReceiveDamage(target.attack == null ? null : target.attack.dmg.GetAndValidateDamage());
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
