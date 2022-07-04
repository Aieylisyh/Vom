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
    }
}