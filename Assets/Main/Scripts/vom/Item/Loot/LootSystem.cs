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

        public Transform lootParent;

        private void Awake()
        {
            instance = this;
            _loots = new List<LootBehaviour>();
        }

        public void SpawnGold(Vector3 pos, ItemData item)
        {
            var loot = Instantiate(prefabGold, lootParent);
            loot.Init(item);
            loot.SetPos(pos);
            _loots.Add(loot);
        }

        public void SpawnSoul(Vector3 pos, ItemData item)
        {
            var loot = Instantiate(prefabSoul, lootParent);
            loot.Init(item);
            loot.SetPos(pos);
            _loots.Add(loot);
        }

        public void Remove(LootBehaviour l, float delay = 0)
        {
            _loots.Remove(l);
            Destroy(l.gameObject, delay);
        }

        public void Clear(bool receiveLoot = false)
        {
            foreach (var l in _loots)
            {
                if (receiveLoot)
                {
                    l.ReceiveLoot(true);
                }
                Destroy(l.gameObject);
            }

            _loots = new List<LootBehaviour>();
        }
    }
}