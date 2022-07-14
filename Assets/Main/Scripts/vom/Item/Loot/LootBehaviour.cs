using UnityEngine;
using com;

namespace vom
{
    public class LootBehaviour : MonoBehaviour
    {
        string _itemId;
        int _amount;
        public PlaySoundBehaviour psb;
        public LootMoveBehaviour move;
        public LootModelSwitcherBehaviour lootModelSwitcher;
        public float removeTime = 0.5f;
        bool _triggered;

        public void Init(ItemData item)
        {
            _itemId = item.id;
            _amount = item.n;
            _triggered = false;

            if (lootModelSwitcher != null)
                lootModelSwitcher.Setup(_itemId, this);
        }

        public void SetPos(Vector3 pos)
        {
            transform.position = pos;
            move.Init(pos, this.lootModelSwitcher);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_triggered || (move != null && !move.CanReceive()))
                return;

            var player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ReceiveLoot(false);
                LootSystem.instance.Remove(this, removeTime);
            }
        }

        public void ReceiveLoot(bool silent)
        {
            _triggered = true;
            InventorySystem.instance.AddItem(new ItemData(_amount, _itemId));

            if (!silent)
                psb.Play();
        }
    }
}