using UnityEngine;

namespace vom
{
    public class LootSubModelBehaviour : MonoBehaviour
    {
        LootBehaviour _loot;
        public string id;

        public LootSubModelBehaviour SetLoot(LootBehaviour l, string lootId)
        {
            _loot = l;
            if (lootId == id)
            {
                gameObject.SetActive(true);
                return this;
            }
            else
            {
                gameObject.SetActive(false);
            }

            return null;
        }

        public void OnTriggerEnter(Collider other)
        {
            _loot.OnTriggerEnter(other);
        }
    }
}