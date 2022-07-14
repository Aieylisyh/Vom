using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class LootSystem : MonoBehaviour
    {
        public static LootSystem instance { get; private set; }

        List<LootBehaviour> _loots;

        public LootBehaviour prefabItem;
        public LootBehaviour prefabSoul;
        public LootBehaviour prefabExp;
        public LootBehaviour prefabLightOrb;
        public LootBehaviour prefabLightOrbBig;
        public Transform lootParent;

        private void Awake()
        {
            instance = this;
            _loots = new List<LootBehaviour>();
        }

        LootBehaviour GetLootPrefab(string id)
        {
            var prefab = prefabLightOrb;
            if (id == "Gold" || id == "Fish" || id == "Apple")
                prefab = prefabItem;
            else if (id == "Soul")
                prefab = prefabSoul;
            else if (id == "Exp")
                prefab = prefabExp;
            else
            {
                var rarity = ItemService.GetPrototype(id).rarity;
                if (rarity == ItemPrototype.Rarity.Epic || rarity == ItemPrototype.Rarity.Legendary)
                    prefab = prefabLightOrbBig;
            }

            return prefab;
        }

        public void SpawnLoot(Vector3 pos, ItemData item)
        {
            var loot = Instantiate(GetLootPrefab(item.id), lootParent);
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