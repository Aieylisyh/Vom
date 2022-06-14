using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class ShipAbilityPrototype : ScriptableObject
    {
        public string id;
        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        public Sprite sp;
        public int intParam;
        public bool hasIntParam = false;

        public int intOtherParam;
        public bool hasIntOtherParam = false;
    }
}
