using System.Collections.Generic;
using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    public class CombatConfig : ScriptableObject
    {
        public PlayerParam playerParam;
        public LevelFieldParam levelFieldParam;
        public SinkParam playerSinkParam;
        public SinkParam enemySinkParam;

        public int reviveMaxCount = 2;

        public AoeParam aoeParam;
        //public AdCabData adCabData;

        [System.Serializable]
        public struct LevelFieldParam
        {
            public float boundRight;
            public float boundLeft;
            public float ShowUpDistX;
            public float z;
            public float ShowUpDistZ;
            public float sizeLengthFactor;
        }

        [System.Serializable]
        public struct PlayerParam
        {
            public string defaultShipId;
            public int maxLevel;
            public Formula expByLevel;//level 1 to 2, param is 0
            public int torTraceNum;
            public int torDirNum;
            public int bombStorage;
            public float bombInterval;
            public Item levelUpBaseReward;
            public int minLevelFromToGainTalentPoint;

            public float lowHealthProtectMinValue;
            public int vignetteShowPercentage;
            public float vignetteShowAlpha;

            public int reviveDiamond1;
            public int reviveDiamond2;
        }

        [System.Serializable]
        public struct SinkParam
        {
            public float acc;
            public float rotateAcc;
            public float rotateZFactor;
            public float rotateYFactor;
            public string effectId;
            public float rotateXSpeedMax;
            public float speedMax;
        }

        [System.Serializable]
        public struct EnduranceParam
        {
            /*
             * 船只耐久度
             * 每只船都有自身的耐久度，使用多次后，需要在船坞中停留一段时间，恢复耐久后才能再次使用
             * 耐久度一开始不显示，通过一定关数后才出现，此时刚好时玩家解锁第二艘船之后
             * 
             * 回复耐久的方式：
             * 使用道具
             * 看广告
             * 不能花费钻石！
             * 
             * 画面反馈方式：
             * 如果耐久度不足，则主场景中，船只会在船坞里面，并且机床在做动作，加上电火花
             * 如果耐久够了，船瞬移出来，没有动画过度
             * 
             */

            public int baseValue;//上限30点
            public int regTimeInSecond;//每3分钟恢复1点
            public int consumeValue;//每次战斗消耗10点
            public int noConsumeTime;// 战斗开始后10秒扣除
        }

        [System.Serializable]
        public class AoeParam
        {
            public float rangeTor = 4.8f;
            public float rangeBomb = 4.9f;
            public float rangeExplodeBomb = 5f;
        }

        [System.Serializable]
        public class AdCabData
        {
            public int countPerSession = 1;
            public bool level1Disable = true;
            public int exposePerSession = 3;
        }
    }
}