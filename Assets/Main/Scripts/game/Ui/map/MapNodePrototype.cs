using UnityEngine;
using System;

namespace game
{
    public class MapNodePrototype
    {
        public string id;
        public string title;
        public string desc;
        public float offsetX;
        public float offsetY;
        public float curveFactor;
        public bool passed;
        public string levelId;

        public NodeViewType nodeViewType;

        public enum NodeViewType
        {
            Normal,
            Elite,
            Boss,
        }
    }
}