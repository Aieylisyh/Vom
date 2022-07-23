using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class SkillPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }
        public virtual string subDesc { get { return id + "_subDesc"; } }

        public string draft;
        public Sprite sp;
        public float cd;

        public float duration;
        public bool isCharge { get { return duration > 0; } }
        public float interval;

        public string sound;

        public List<SkillPerkPrototype> perks;

        public int attri1;
        public int attri2;

        public int fixedAttri1;
        public int fixedAttri2;
    }
}