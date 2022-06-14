using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class TalentPanelBehaviour : MonoBehaviour
    {
        public TalentSlotsBehaviour  slots;

        public TalentCategory category;

        public static TalentPanelBehaviour currentDisplayingInstance;

        public void Setup()
        {
            foreach (var s in slots.slots)
            {
                s.category = category;
                s.Refresh(false);
            }
        }

        public void Refresh()
        {
            //Debug.Log("Refresh!");
            foreach (var s in slots.slots)
            {
                s.Refresh(true);
            }
        }
    }
}
