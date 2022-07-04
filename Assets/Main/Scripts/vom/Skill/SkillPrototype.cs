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

        public int tierUnlock = 0;

        public List<SkillPerkPrototype> perks;

        public int attri1;
        public int attri2;
        public int attri3;

        public int fixedAttri1;
        public int fixedAttri2;
        public int fixedAttri3;
        public int fixedAttri4;

        public float cd;
        public int slot;//0 1 2
    }
}