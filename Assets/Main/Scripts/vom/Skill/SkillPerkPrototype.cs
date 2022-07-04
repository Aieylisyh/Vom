using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class SkillPerkPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }
        public virtual string subDesc { get { return id + "_subDesc"; } }

        public string draft;

        public List<ItemData> price;

        public int fixedAttri1;
        public int fixedAttri2;
    }
}