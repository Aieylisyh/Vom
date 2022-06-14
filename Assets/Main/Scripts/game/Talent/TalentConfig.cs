using System.Collections.Generic;
using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    public class TalentConfig : ScriptableObject
    {
        public int initialPoints;

        public List<Item> resetDiamondPrices;

        public List<TalentPrototype> list = new List<TalentPrototype>();
    }
}
