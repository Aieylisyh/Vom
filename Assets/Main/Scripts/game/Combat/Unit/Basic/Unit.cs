using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class Unit : MonoBehaviour
    {
        public UnitAi ai;
        public UnitAttack attack;
        public UnitDeath death;
        public UnitMove move;
        public UnitHealth health;
        public UnitReward reward;
        public UnitRevive revive;
        public UnitCollision collision;

        public ActivableSpecialModule[] asms;

        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
            ai = (ai != null) ? ai : GetComponent<UnitAi>();
            attack = (attack != null) ? attack : GetComponent<UnitAttack>();
            death = (death != null) ? death : GetComponent<UnitDeath>();
            move = (move != null) ? move : GetComponent<UnitMove>();
            health = (health != null) ? health : GetComponent<UnitHealth>();
            reward = (reward != null) ? reward : GetComponent<UnitReward>();
            revive = (revive != null) ? revive : GetComponent<UnitRevive>();
            collision = (collision != null) ? collision : GetComponent<UnitCollision>();
        }

        public void ResetComponentState()
        {
            ai?.ResetState();
            attack?.ResetState();
            death?.ResetState();
            move?.ResetState();
            health?.ResetState();
            reward?.ResetState();
            revive?.ResetState();
            collision?.ResetState();
            //CheckComp();
        }

        private void CheckComp()
        {
            if (ai == null)
            {
                Debug.LogWarning("no ai");
            }
            if (attack == null)
            {
                Debug.LogWarning("no attack");
            }
            if (death == null)
            {
                Debug.LogWarning("no death");
            }
            if (move == null)
            {
                Debug.LogWarning("no move");
            }
            if (health == null)
            {
                Debug.LogWarning("no health");
            }
            if (reward == null)
            {
                Debug.LogWarning("no reward");
            }
            if (revive == null)
            {
                Debug.LogWarning("no revive");
            }
            if (collision == null)
            {
                Debug.LogWarning("no collision");
            }
        }

        public virtual void Recycle()
        {
            Debug.LogWarning("raw Recycle!");
        }

        public bool IsAlive()
        {
            if (this == null)
                return false;

            if (gameObject == null)
                return false;

            if (!gameObject.activeSelf)
                return false;

            if (death != null)
                return !death.isDead;

            if (health != null)
                return health.HasRemainingHp();

            return true;
        }


        public void ToggleSpecialModule(bool active)
        {
            foreach (var asm in asms)
            {
                asm?.ToggleSpecialModule(active);
            }
        }
    }
}
