using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class LevelPrototype : ScriptableObject
    {
        public string id;
        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        public int enemyLevel;
        public bool needUnlock = false;

        public Item unlockPrice;

        public LevelType levelType = LevelType.Campaign;

        public List<Item> firstPassReward;
        public List<WavePrototype> waves;
        public MapViewData mapViewData;
        public RaidData raidData;
        public bool disableNextLevel;
        public int waveEnemyLevelAdd;
        public List<Item> GetFirstPassReward()
        {
            List<Item> res = new List<Item>();
            foreach (var r in firstPassReward)
            {
                if (r.n > 0)
                {
                    res.Add(new Item(r.n, r.id));
                }
            }
            return res;
        }

        public List<Item> GetRaidRawReward()
        {
            List<Item> res = new List<Item>();
            foreach (var r in raidData.reward)
            {
                if (r.n > 0)
                {
                    res.Add(new Item(r.n, r.id));
                }
            }
            return res;
        }

        public enum LevelType
        {
            Campaign,
            Boss,
            Abysse,
            Extra,
        }

        public int waveCount
        {
            get { return waves.Count; }
        }
    }

    [System.Serializable]
    public struct RaidData
    {
        public List<Item> reward;
    }

    [System.Serializable]
    public struct MapViewData
    {
        public MapNodePrototype.NodeViewType nodeViewType;

        public float offsetX;
        public float offsetY;
        public float curveFactor;
        public int displayMinPassedLevelIndex;
    }

    [System.Serializable]
    public struct LevelEvent
    {
        public string evt;
        public float time;

        public List<float> floatParam;
        public List<string> stringParam;
        public List<bool> boolParam;
    }
}