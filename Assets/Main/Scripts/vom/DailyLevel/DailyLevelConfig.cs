using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class DailyLevelConfig : ScriptableObject
    {
        public List<DailyPerkPrototype> perks;

        public com.Formula experienceRequirement;
    }
}
