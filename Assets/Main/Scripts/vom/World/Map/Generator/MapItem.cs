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

        public void Start()
        {
            foreach (var c in connectors)
            {
                c.fromId = mapId;
            }

            sizeX = tiles[tiles.Count - 1].x + 1;
            sizeZ = tiles[tiles.Count - 1].z + 1;
        }
    }
}