using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class LootSystem : MonoBehaviour
    {
        public static LootSystem instance { get; private set; }

        List<LootBehaviour> _loots;

        private void Awake()
        {
            instance = this;
            _loots = new List<LootBehaviour>();
        }

        public void Add(LootBehaviour l)
        {
            _loots.Add(l);
        }

        public void Remove(LootBehaviour l)
        {
            _loots.Remove(l);
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