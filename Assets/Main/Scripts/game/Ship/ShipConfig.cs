using System.Collections.Generic;
using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class ShipConfig : ScriptableObject
    {
        public List<ShipPrototype> ships = new List<ShipPrototype>();
    }
}