using System.Collections.Generic;
using UnityEngine;
using com;

namespace vom
{
    [CreateAssetMenu]
    public class TalentConfig : ScriptableObject
    {
        public int initialPoints;

        public List<ItemData> resetDiamondPrices;

        public List<TalentPrototype> list = new List<TalentPrototype>();
    }
}
