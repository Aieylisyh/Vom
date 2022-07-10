using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class DropPrototype : ScriptableObject
    {
        public int pick;
        public List<DropPrototype> subs;

        public ItemData item;
    }
}
