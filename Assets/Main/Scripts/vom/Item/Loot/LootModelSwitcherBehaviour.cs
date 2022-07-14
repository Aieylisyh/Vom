using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class LootModelSwitcherBehaviour : MonoBehaviour
    {
        public List<LootSubModelBehaviour> defs;

        public LootSubModelBehaviour crt { get; private set; }

        public void Setup(string id, LootBehaviour loot)
        {
            foreach (var d in defs)
            {
                var m = d.SetLoot(loot, id);
                if (m != null)
                {
                    crt = m;
                }
            }
        }
    }
}