using UnityEngine;

namespace vom
{
    public class SpawnLootableBehaviour : PropsSpawnBehaviour
    {
        public ItemData itemData;

        protected override void Spawn()
        {
            var go = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
            var loot = go.GetComponent<LootBehaviour>();
            loot.Init(itemData);
            Destroy(gameObject);
        }
    }
}