using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class LootSystem : MonoBehaviour
    {
        public static LootSystem instance { get; private set; }

        List<LootBehaviour> _loots;

        public LootBehaviour prefab;
        public Transform lootParent;

        private void Awake()
        {
            instance = this;
            _loots = new List<LootBehaviour>();
        }

        public void Spawn(Vector3 pos, ItemData item, int index = 0)
        {
            var loot = Instantiate(prefab, pos, Quaternion.identity, lootParent);
            loot.Init(item, index);
            _loots.Add(loot);
        }

        public void Remove(LootBehaviour l)
        {
            _loots.Remove(l);
            Destroy(l.gameObject);
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