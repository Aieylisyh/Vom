using UnityEngine;
using com;
using System.Collections.Generic;

namespace game
{
    public class CombatAbilityInventoryBehaviour : MonoBehaviour
    {
        public List<CombatAbilitySlotBehaviour> slots;
        public bool isProceedOrPool;
        public bool hideEmpty = false;
        public bool toRefresh = false;

        private void CheckHideLines(int count)
        {
            if (!hideEmpty)
                return;

            int slotCount = 0;
            foreach (var s in slots)
            {
                s.gameObject.SetActive(slotCount < count);
                slotCount++;
            }
        }

        private void Update()
        {
            if (toRefresh)
            {
                toRefresh = false;
                Refresh();
            }
        }

        public void Refresh()
        {
            List<string> cabIds;
            if (isProceedOrPool)
            {
                cabIds = CombatAbilityService.instance.selectedPool;
            }
            else
            {
                cabIds = CombatAbilityService.instance.totalPool;
            }

            CheckHideLines(cabIds.Count);

            int i = -1;
            foreach (var s in slots)
            {
                i++;
                if (cabIds.Count > i)
                {
                    s.Setup(cabIds[i], isProceedOrPool);
                }
                else
                {
                    s.SetEmpty();
                }
            }
        }
    }
}
