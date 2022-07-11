using UnityEngine;

namespace vom
{
    public class PlayerSkillBehaviour : VomPlayerComponent
    {
        public void CastSpellAnim()
        {
            host.interaction.HideAll();
            host.animator.SetTrigger(PlayerAnimeParams.cast);
        }

        public void CastSpellBigAnim()
        {
            host.interaction.HideAll();
            host.animator.SetTrigger(PlayerAnimeParams.castBig);
        }

        public void CastChargeAnim(bool b)
        {
            host.interaction.HideAll();
            if (b)
                host.animator.SetTrigger(PlayerAnimeParams.chargeStart);
            else
                host.animator.SetTrigger(PlayerAnimeParams.chargeEnd);
        }
    }
}