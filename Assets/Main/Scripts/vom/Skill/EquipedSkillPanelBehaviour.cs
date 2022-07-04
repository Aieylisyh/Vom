using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EquipedSkillPanelBehaviour : MonoBehaviour
    {
        public static EquipedSkillPanelBehaviour instance { get; private set; }

        public List<EquipedSkillSlotBehaviour> slots;

        private void Awake()
        {
            instance = this;
        }

        public void OnClickSkill1()
        {
            if (SkillSystem.instance.CanUseSkill1())
            {
                SkillSystem.instance.UseSkill1();
            }
        }

        public void OnClickSkill2()
        {
            if (SkillSystem.instance.CanUseSkill2())
            {
                SkillSystem.instance.UseSkill2();
            }
        }

        public void OnClickSkill3()
        {
            if (SkillSystem.instance.CanUseSkill3())
            {
                SkillSystem.instance.UseSkill3();
            }
        }
    }
}