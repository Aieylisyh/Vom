using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class HeroPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }
        public virtual string subDesc { get { return id + "_subDesc"; } }

        public List<SkillPrototype> skills;

        public AttributeLayerData attributes;
    }
}