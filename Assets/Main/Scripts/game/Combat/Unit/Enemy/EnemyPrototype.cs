using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    [CreateAssetMenu]
    [System.Serializable]
    public class EnemyPrototype : ScriptableObject
    {
        public string id;

        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        public EnemyCategory Category;

        public float speed;
        public int attack;
        public int hp;
        public EnemyLevelupData levelupData;
        public EnemyDropData dropData;

        public int GetAttack(int level = 0)
        {
            return MathGame.GetPercentageAdded(attack, level * levelupData.attackBoost);
        }

        public int GetHp(int level = 0)
        {
            return MathGame.GetPercentageAdded(hp, level * levelupData.hpBoost);
        }

        public int GetExp(int level = 0)
        {
            return MathGame.GetPercentageAdded(dropData.exp, level * levelupData.dropBoost);
        }

        public List<Item> GetDrops(int level = 0)
        {
            List<Item> newList = new List<Item>();

            foreach (var d in dropData.dropItems)
            {
                if (Random.value >= d.chance)
                    continue;

                int amount = d.item.n;
                if (!d.amountNotAffectByRatio)
                    amount = MathGame.GetPercentageAdded(amount, (float)level * levelupData.dropBoost);

                newList.Add(new Item(amount, d.item.id));
            }

            return newList;
        }

        [System.Serializable]
        public struct EnemyLevelupData
        {
            public float dropBoost;
            public float attackBoost;
            public float hpBoost;
        }

        [System.Serializable]
        public struct EnemyDropData
        {
            public int score;
            public int exp;
            public List<DropItem> dropItems;
        }

        [System.Serializable]
        public struct DropItem
        {
            public float chance;
            public Item item;
            public bool amountNotAffectByRatio;
        }

        [System.Serializable]
        public struct EnemyCategory
        {
            public enum Tier
            {
                Light,
                Heavy,
                Boss,
            };

            public bool isMechanical;
            public Tier tier;
        }
    }
}