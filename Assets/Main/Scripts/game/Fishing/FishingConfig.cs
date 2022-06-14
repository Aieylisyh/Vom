using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    [CreateAssetMenu]
    public class FishingConfig : ScriptableObject
    {
        public List<string> defaultFish;

        public float accReturnSecond = 5;

        public List<FishingPrototype> rftPrototypes;
        public FishingPrototype defaultRftPrototype;

        public float noAccTimeSec = 30;
        public float freeAccTimeSec = 60;

        public float accTimeSecPerDiamond = 900;

        public List<int> fishPickWeight;
        public List<string> fishToPick;

        public BoatLevelData boatLevelData;
        public List<FishingAbilityUnlockPrototype> abilityUnlocks;

        public float extraPickWeight = 0;//will receive 50% amount bonus from boat level 
        public List<Item> extraItems;//each rft will pick [extraPickWeight] random elements from this list

        public AdAccData adAccData;
    }

    [System.Serializable]
    public class AdAccData
    {
        public int countPerSession;
        public int lowCostCap;
        public int highCostCap;
    }

    [System.Serializable]
    public class BoatLevelData
    {
        public int maxLevel;
        public Formula rftDurationAddPercent;
        public Formula rftAmount;
        public Formula goldCost;
    }

    [System.Serializable]
    public class FishingAbilityUnlockPrototype
    {
        public List<Item> price;
        public FishingAbilityPrototype ability;
        public int boatLevelRequire;
    }
}