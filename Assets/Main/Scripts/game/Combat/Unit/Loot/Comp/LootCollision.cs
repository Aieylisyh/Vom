using UnityEngine;

namespace game
{
    public class LootCollision : UnitCollision
    {
        protected override void OnCollided(Unit target)
        {
            base.OnCollided(target);
            (self as Loot).PlayLootSound();
            self.Recycle();
        }

        protected override void OnCollidedKillzone()
        {
        }
    }
}
