using UnityEngine;

namespace vom
{
    public class PlayerCombatStateBehaviour : VomPlayerComponent
    {
        public CanvasGroup cg1;
        public CanvasGroup cg2;

        bool _showHud;
        public float showHudSpeed = 4;

        public bool isInCombat
        {
            get
            {
                if (host.health.dead)
                {
                    return false;
                }
                return host.attack.HasAliveTarget || IsTargeted;
            }
        }

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
            if (show)
            {
                cg1.blocksRaycasts = true;
                cg1.interactable = true;
                cg2.blocksRaycasts = true;
                cg2.interactable = true;
            }
            else
            {
                cg1.blocksRaycasts = false;
                cg1.interactable = false;
                cg2.blocksRaycasts = false;
                cg2.interactable = false;
            }
        }

        public void UpdateState()
        {
            if (host.health.dead)
            {
                host.interaction.HideAll();
                ShowHud(false);
                return;
            }

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
            if (cg1.alpha > 0 && !_showHud)
            {
                cg1.alpha -= Time.deltaTime * showHudSpeed;
                if (cg1.alpha < 0)
                    cg1.alpha = 0;
            }
            else if (cg1.alpha < 1 && _showHud)
            {
                cg1.alpha += Time.deltaTime * showHudSpeed;
                if (cg1.alpha > 1)
                    cg1.alpha = 1;
            }

            if (cg2.alpha > 0 && !_showHud)
            {
                cg2.alpha -= Time.deltaTime * showHudSpeed;
                if (cg2.alpha < 0)
                    cg2.alpha = 0;
            }
            else if (cg2.alpha < 1 && _showHud)
            {
                cg2.alpha += Time.deltaTime * showHudSpeed;
                if (cg2.alpha > 1)
                    cg2.alpha = 1;
            }
        }
    }
}