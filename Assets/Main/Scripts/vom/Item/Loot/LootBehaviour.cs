using UnityEngine;
using com;

namespace vom
{
    public class LootBehaviour : MonoBehaviour
    {
        public string itemId;
        public int amount;
        public PlaySoundBehaviour psb;
        public PickByString pbs;
        //public ParticleSystem ps;
        public LootMoveBehaviour move;

        public void Init(ItemData item, int dropIndex)
        {
            itemId = item.id;
            amount = item.n;
            move.Init(dropIndex);
            pbs?.Setup(itemId);
        }

        public void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ReceiveLoot(false);
                LootSystem.instance.Remove(this);
            }
        }

        public void ReceiveLoot(bool silent)
        {
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