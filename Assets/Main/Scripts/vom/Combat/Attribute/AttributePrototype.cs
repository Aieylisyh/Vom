using UnityEngine;

namespace vom
{
    [System.Serializable]
    public class AttributePrototype
    {
        public string id;

        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        public int value;
    }
}