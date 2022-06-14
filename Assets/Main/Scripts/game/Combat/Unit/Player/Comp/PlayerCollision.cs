using UnityEngine;

namespace game
{
    public class PlayerCollision : UnitCollision
    {
        protected override void OnCollided(Unit target)
        {
            base.OnCollided(target);

            if (self.health != null)
            {
                //Debug.Log("player OnCollided by " + target.gameObject.name);
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

        protected override void OnCollidedKillzone()
        {
            //Killzone don't kill the player
        }
    }
}
