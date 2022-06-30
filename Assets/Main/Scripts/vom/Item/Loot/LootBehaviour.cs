using UnityEngine;
using com;

namespace vom
{
    public class LootBehaviour : MonoBehaviour
    {
        public string itemId;
        public int amount;
        public Transform viewParent;//2d 3d?
        public PlaySoundBehaviour psb;
        public PickByString pbs;
        public ParticleSystem ps;
        public LootMoveBehaviour move;

        public void Init(ItemData item, int dropIndex)
        {
            itemId = item.id;
            amount = item.n;
            move.Init(dropIndex);
            pbs?.Setup(itemId);
            LootSystem.instance.Add(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ReceiveLoot(false);
            }
        }

        public void ReceiveLoot(bool silent)
        {
            InventorySystem.instance.AddItem(itemId, amount);

            if (!silent)
            {
                ps.Play();
                psb.Play();
            }
        }
    }
}