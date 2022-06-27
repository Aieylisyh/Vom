using UnityEngine;
using game;

namespace vom
{
    public class PlayerCombatStateBehaviour : VomPlayerComponent
    {
        public bool isInCombat { get { return host.attack.HasTarget || IsTargeted; } }

        public bool IsTargeted
        {
            get
            {
                return EnemySystem.instance.HasEnemyTargetedPlayer();
            }
        }
    }
}