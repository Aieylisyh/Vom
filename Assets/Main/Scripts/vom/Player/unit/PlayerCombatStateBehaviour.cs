using UnityEngine;

namespace vom
{
    public class PlayerCombatStateBehaviour : VomPlayerComponent
    {
        public CanvasGroup[] cgs;

        bool _showHud;
        public float showHudSpeed = 4;

        public bool isInCombat
        {
            get
            {
                if (host.health.dead)
                    return false;
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
            Debug.Log("ShowHud " + show);
            _showHud = show;
            if (show)
            {
                foreach (var cg in cgs)
                    ToggleCanvasGroup(cg, true);
            }
            else
            {
                foreach (var cg in cgs)
                    ToggleCanvasGroup(cg, false);
            }
        }

        void ToggleCanvasGroup(CanvasGroup cg, bool b)
        {
            cg.blocksRaycasts = b;
            cg.interactable = b;
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
            foreach (var cg in cgs)
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
}