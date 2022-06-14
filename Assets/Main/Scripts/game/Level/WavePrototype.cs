using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class WavePrototype : ScriptableObject
    {
        public List<LevelEvent> events;
        public List<SimpleEnemyEvent> enemies;

        public List<LevelEvent> GetEvents()
        {
            var res = new List<LevelEvent>();
            res.AddRange(events);
            float delay = 0;
            foreach (var e in enemies)
            {
                var evt = new LevelEvent();
                evt.boolParam = new List<bool>();
                evt.boolParam.Add(e.right);
                evt.boolParam.Add(false);//showupFromNear
                evt.stringParam = new List<string>();
                var eneId = e.enemyType.ToString();
                evt.stringParam.Add(eneId);
                evt.floatParam = new List<float>();
                evt.floatParam.Add(e.height);
                evt.evt = "ene";
                delay += e.delay;
                evt.time = delay;
                res.Add(evt);
            }

            return res;
        }

        public bool disableCab;
        public int enemyLevelExtraOffset;
    }

    public enum EnemyType
    {
        Normal,
        Fast,
        Weak,
        Common,
        Stealth,
        Lurker,
        Veteran,
        Marshal,
        Defender,
        Angel,
        Tiny,
        Bonus,
        Phantom,
        Laser,
        Shield,
        Summoner,
        Ghost,
        Back,
        Tough,
        Micro,
        BossCannon,
        BossLaser,
        BossGhost,
        BossSwift,
        BossWeak,
    }

    [System.Serializable]
    public struct SimpleEnemyEvent
    {
        public bool right;

        public EnemyType enemyType;

        [Range(4, 12)]
        public float height;

        public float delay;
    }
}