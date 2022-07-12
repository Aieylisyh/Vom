using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class LootSystem : MonoBehaviour
    {
        public static LootSystem instance { get; private set; }

        List<LootBehaviour> _loots;

        public LootBehaviour prefabGold;
        public LootBehaviour prefabSoul;
        public LootBehaviour prefabExp;
        public LootBehaviour prefabLightOrb;

        public Transform lootParent;

        private void Awake()
        {
            instance = this;
            _loots = new List<LootBehaviour>();
        }

        public void SpawnLoot(Vector3 pos, ItemData item)
        {
            var prefab = prefabLightOrb;
            if (item.id == "Gold")
                prefab = prefabGold;
            else if (item.id == "Soul")
                prefab = prefabSoul;
            else if (item.id == "Exp")
                prefab = prefabExp;

            var loot = Instantiate(prefab, lootParent);
            loot.Init(item);
            loot.SetPos(pos);
            _loots.Add(loot);
        }

        public void Remove(LootBehaviour l, float delay = 0.5f)
        {
            _loots.Remove(l);
            Destroy(l.gameObject, delay);
        }

        public void Clear(bool receiveLoot = false)
        {
            foreach (var l in _loots)
            {
                if (receiveLoot)
                    l.ReceiveLoot(true);
                Destroy(l.gameObject);
            }

            _loots = new List<LootBehaviour>();
        }
    }
}