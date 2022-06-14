using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class BossSkillPrototype : ScriptableObject
    {
        public float duration = 0;
        public string id;
        public string desc;

        public bool isTimedSkill;

        public float damageRatio;
        public float hpRatio;
        public int exclusiveGroupId;
        public int attackPosId;
        public float interval;
        public int attackCountMax;

        public float paramFloat1;
        public bool paramBool1;
        public string paramString1;
        public int paramInt1;

        public AttackMode attackMode;
        public enum AttackMode
        {
            TorSingle,
            TorRing,
            Laser,
            Summon,
            Teleport,
        }
    }
}
