using UnityEngine;

namespace game
{
    public class ProjectileDeath : UnitDeath
    {
        public string dieEffectId = "exp projectile";
        public string overrideDieEffectId;

        protected override void DieUnsilent()
        {
            //Debug.Log(gameObject + "ProjectileDeath DieUnsilent");
            SpawnEffect(string.IsNullOrEmpty(overrideDieEffectId) ? dieEffectId : overrideDieEffectId);
            base.DieUnsilent();
        }
    }
}