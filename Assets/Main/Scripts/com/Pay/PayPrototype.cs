using UnityEngine;
using com;
using UnityEngine.Purchasing;

namespace com
{
    [CreateAssetMenu]
    public class PayPrototype : ScriptableObject
    {
        public string commodityId;
        public string iapId;
        public string desc;
        public ProductType type;

    }
}