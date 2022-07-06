using UnityEngine;

namespace vom
{
    public class PlayerSkillBehaviour : VomPlayerComponent
    {
        public void CastSpellAnim()
        {
            host.interaction.HideAll();
            host.animator.SetTrigger("cast");
        }

        public void CastSpellBigAnim()
        {
            host.interaction.HideAll();
            host.animator.SetTrigger("castBig");
        }

        public void CastChargeAnim(bool b)
        {
            host.interaction.HideAll();
            if (b)
                host.animator.SetTrigger("chargeStart");
            else
                host.animator.SetTrigger("chargeEnd");
        }
    }
}