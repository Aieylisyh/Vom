using UnityEngine;
using System;

namespace game
{
    [Serializable]
    public class MapNodeData
    {
        public MapNodeBaseData passed;
        public MapNodeBaseData raw;
    }
    [Serializable]
    public struct MapNodeBaseData
    {
        public float width;
        public Sprite sp;
    }
}
