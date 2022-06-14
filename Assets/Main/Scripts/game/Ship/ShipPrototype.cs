using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    [CreateAssetMenu]
    public class ShipPrototype : ScriptableObject
    {
        public string id;
        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }
        public string subDesc { get { return id + "_subDesc"; } }

        public int levelMax;
        public Item price;
        public Formula levelupGoldCost;

        public Formula hp;
        public Formula speed;
        public Formula torDmg;
        public Formula bombDmg;
        public Formula reg;
        public Formula vita;

        public Formula armor;
        public Formula armor_hotWeapon;
        public Formula armor_laser;
        public Formula armor_ghost;
        public Formula dmgReduce;
        public Formula dmgReduce_hotWeapon;
        public Formula dmgReduce_laser;
        public Formula dmgReduce_ghost;

        public List<ShipAbilityUnlockPrototype> abilityUnlocks;

        public int GetAbilityCardLevelByIndex(int index)
        {
            int res = 1;
            string id = abilityUnlocks[index].ability.id;
            for (int i = 1; i < abilityUnlocks.Count; i++)
            {
                if (i > index)
                {
                    break;
                }

                var abu = abilityUnlocks[i];
                if (abu.ability.id == id)
                {
                    res++;
                }
            }

            return res;
        }

        [System.Serializable]
        public class ShipAbilityUnlockPrototype
        {
            public List<Item> price;
            public ShipAbilityPrototype ability;
            public int shipLevelRequire;
        }
    }
}