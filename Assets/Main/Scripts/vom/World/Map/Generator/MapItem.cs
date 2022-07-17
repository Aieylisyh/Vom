using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class MapItem : MonoBehaviour
    {
        /// <summary>
        /// map id is unique per map.
        /// while mapprototype id is unique per mapPrototype,
        /// same mapPrototype looks similar to players(bg color, map name) but can be different map
        /// </summary>
        public string mapId;

        public List<TileCacheBehaviour.OutputTileData> tiles;
        public MapPrototype prototype;
        public Transform playerStart;
        public BlessingBehaviour blessing;

        public List<MapConnectorPrototype> connectors;

        [HideInInspector]
        public int sizeX;
        [HideInInspector]
        public int sizeZ;
        [HideInInspector]
        public int offsetX;
        [HideInInspector]
        public int offsetZ;
    }
}