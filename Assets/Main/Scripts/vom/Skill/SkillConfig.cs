using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class SkillConfig : ScriptableObject
    {
        public List<SkillPrototype> skills;

        public float globalModifier;//TODO use it

    }
}