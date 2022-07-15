using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class AttributesConfig : ScriptableObject
    {
        public List<AttributePrototype> attributes;
    }
}