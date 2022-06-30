using UnityEngine;

namespace vom
{
    public class PlayerCombatStateBehaviour : VomPlayerComponent
    {
        public CanvasGroup cg;

        bool _showHud;
        public float showHudSpeed = 4;

        public bool isInCombat { get { return host.attack.HasAliveTarget || IsTargeted; } }

        public bool IsTargeted
        {
            get
            {
                return EnemySystem.instance.HasEnemyTargetedPlayer();
            }
        }

        public void ShowHud(bool show)
        {
            _showHud = show;
        }

        public void UpdateState()
        {
            if (isInCombat)
            {
                host.interaction.HideAll();
                ShowHud(false);
            }
            else
            {
                host.interaction.ShowAll();
                ShowHud(true);
            }
        }

        protected override void Update()
        {
            if (cg.alpha > 0 && !_showHud)
            {
                cg.alpha -= Time.deltaTime * showHudSpeed;
                if (cg.alpha < 0)
                    cg.alpha = 0;
            }
            else if (cg.alpha < 1 && _showHud)
            {
                cg.alpha += Time.deltaTime * showHudSpeed;
                if (cg.alpha > 1)
                    cg.alpha = 1;
            }
        }
    }
}