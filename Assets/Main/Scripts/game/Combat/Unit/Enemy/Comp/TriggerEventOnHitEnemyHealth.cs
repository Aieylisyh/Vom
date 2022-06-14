using UnityEngine;

namespace game
{
    public class TriggerEventOnHitEnemyHealth : EnemyHealth
    {
        public int evtIndex = 0;

        protected override void OnHealthChange(int v)
        {
            if (v < 0)
            {
                var ai = self.ai as EnemyAi;
                ai.ForceTriggerEvent(evtIndex);
            }

            base.OnHealthChange(v);
        }
    }
}
