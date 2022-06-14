using System.Collections.Generic;
using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class TechniqueConfig : ScriptableObject
    {
        //暂时取消科技
        public List<TechniquePrototype> list = new List<TechniquePrototype>();
        
    }
}