using UnityEngine;
using game;

namespace vom
{
    public class PlayerCombatStateBehaviour : VomPlayerComponent
    {
        public bool isInCombat { get { return host.attack.HasAliveTarget || IsTargeted; } }

        public bool IsTargeted
        {
            get
            {
                return EnemySystem.instance.HasEnemyTargetedPlayer();
            }
        }

        public void UpdateState()
        {
            if (isInCombat)
            {
                host.interaction.HideAll();
            }
            else
            {
                host.interaction.ShowAll();
            }
        }
    }
}