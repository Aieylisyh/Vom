using System.Collections.Generic;
using com;
using UnityEngine;

namespace vom
{
    [System.Serializable]
    public class AttributeLayerPrototype : ScriptableObject
    {
        public string id;

        public List<AttributePrototype> attributes;

        public void Sync()
        {

        }
    }
}