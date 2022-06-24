using System.Collections.Generic;
using UnityEngine;
using com;

namespace vom
{
    [CreateAssetMenu]
    public class MapPrototype : ScriptableObject
    {
        public string id;

        public string titleKey
        {
            get
            {
                return "map_" + id;
            }
        }

        public string title
        {
            get
            {
                return id;//TODO
            }
        }
        public Color bgColor;

        public Biome biome;
    }

    public enum Biome
    {
        Grass ,
        Forest,
        Frost,
        Grave,
        Desert,
        Rocky,
    }
}