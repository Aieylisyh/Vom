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

        public void Init(ItemData item)
        {
            itemId = item.id;
            amount = item.n;
            _triggered = false;
        }

        public void SetPos(Vector3 pos)
        {
            transform.position = pos;
            move.Init(pos);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_triggered || !move.CanReceive())
                return;

            var player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ReceiveLoot(false);
                LootSystem.instance.Remove(this);
            }
        }

        public void ReceiveLoot(bool silent)
        {
            _triggered = true;
            InventorySystem.instance.AddItem(new ItemData(amount, itemId));

            if (!silent)
                psb.Play();
        }
    }
}