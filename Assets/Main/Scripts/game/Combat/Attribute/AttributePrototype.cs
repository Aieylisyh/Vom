using UnityEngine;
using System.Collections;
namespace game
{

    [System.Serializable]
    public class AttributePrototype
    {
        public string id;

        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        //public Sprite image;

        //Sprite
        //public DamageType damageType;
    }
}