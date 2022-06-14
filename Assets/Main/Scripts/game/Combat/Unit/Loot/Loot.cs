using UnityEngine;
using com;

namespace game
{
    public class Loot : Unit
    {
        public string itemId;
        public int amount;
        public Transform viewParent;//2d 3d?
        public PlaySoundBehaviour psb;
        public PickByString pbs;

        public LootMove lootMove
        {
            get
            {
                return move as LootMove;
            }
        }

        public void Init(Item item, int dropIndex)
        {
            itemId = item.id;
            amount = item.n;
            lootMove.dropIndex = dropIndex; //the bigger dropIndex, the farer loot spawn

            CombatService.instance.Register(this);
            ResetComponentState();
            pbs?.Setup(itemId);
        }

        public override void Recycle()
        {
            LevelService.instance.AddLevelLoot(itemId, amount);
            PoolingService.instance.Recycle(this);
        }

        public void PlayLootSound()
        {
            psb.Play();
        }
    }
}
