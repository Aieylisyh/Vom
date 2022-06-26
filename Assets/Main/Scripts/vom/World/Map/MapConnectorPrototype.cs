using UnityEngine;
using System;

namespace vom
{
    [System.Serializable]
    public class MapConnectorPrototype
    {
        public enum ConnectToType
        {
            Right,
            Left,
            Forward,
            Backward,
        }

        public ConnectToType type;

        public string toId;

        [Range(0f, 1f)]
        public float posPercentage;

        public int posOffset;

        [HideInInspector]
        public string fromId;
    }
}