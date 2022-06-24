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
            Up,
            Down,
        }

        public ConnectToType type;
        public string toId;
        public string fromId;
    }
}