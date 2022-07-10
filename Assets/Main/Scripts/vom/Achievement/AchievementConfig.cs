using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class AchievementConfig : ScriptableObject
    {
        public List<AchievementPrototype> list;

        public string soundAchieve;
    }
}