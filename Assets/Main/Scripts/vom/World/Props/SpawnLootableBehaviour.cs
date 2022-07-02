using UnityEngine;

namespace vom
{
    public class SpawnLootableBehaviour : RuntimeSpawnBehaviour
    {
        public ItemData itemData;

        protected override void Spawn()
        {
            var go = Instantiate(prefab, transform.position, transform.rotation, transform.parent);
            var loot = go.GetComponent<LootBehaviour>();
            loot.Init(itemData);
            Destroy(gameObject);
        }
    }
}