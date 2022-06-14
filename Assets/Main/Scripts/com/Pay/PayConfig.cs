using UnityEngine;
using System.Collections.Generic;

namespace com
{
    [CreateAssetMenu]
    public class PayConfig : ScriptableObject
    {
        public bool testMode;

        public List<PayPrototype> pays;
    }
}
