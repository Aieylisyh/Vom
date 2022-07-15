using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class AttributePrototype : ScriptableObject
    {
        public string id;

        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        public Sprite sp;

        public List<AttributeApplyMethod> applyMethods;
    }

    [System.Serializable]
    public struct AttributeApplyMethod
    {
        public enum Method
        {
            Add = 1,//+1
            Minus = 3,//-1
            Add_Multiply = 7,//+1%
            Minus_Multiply = 9,//-1%
        }

        public string tId;//targetAttributeId, often the same as AttributePrototype
        public Method method;
    }
}