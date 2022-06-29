using UnityEngine;

namespace vom
{
    public class PlayerSkillBehaviour : VomPlayerComponent
    {
        public void CastSpell()
        {
            host.interaction.HideAll();
            host.animator.SetTrigger("cast");
        }

        public void CastSpellBig()
        {
            host.interaction.HideAll();
            host.animator.SetTrigger("castBig");
        }
    }
}