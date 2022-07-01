using UnityEngine;
using com;

namespace vom
{
    public class LootBehaviour : MonoBehaviour
    {
        public string itemId;
        public int amount;
        public PlaySoundBehaviour psb;
        public LootMoveBehaviour move;

        bool _triggered;

        public void Init(ItemData item, int dropIndex, Vector3 pos)
        {
            itemId = item.id;
            amount = item.n;
            move.Init(dropIndex, pos);
            _triggered = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;

            var player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ReceiveLoot(false);
                LootSystem.instance.Remove(this, 0.6f);
            }
        }

        public void ReceiveLoot(bool silent)
        {
            _triggered = true;
            InventorySystem.instance.AddItem(itemId, amount);
            PlayerBehaviour.instance.health.Heal(1);//TODO
            if (!silent)
            {
                //ps.Play();
                psb.Play();
            }
        }
    }
}