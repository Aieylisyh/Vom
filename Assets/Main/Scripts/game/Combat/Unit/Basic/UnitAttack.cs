using UnityEngine;
using com;

namespace game
{
    public class UnitAttack : UnitComponent
    {
        public Damage dmg;
        public string attackSound;

        public virtual void Attack()
        {
            SoundService.instance.Play(attackSound);
        }
    }
}
