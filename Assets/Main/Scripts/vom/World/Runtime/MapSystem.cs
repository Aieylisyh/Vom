using UnityEngine;
using System.Collections.Generic;
using com;
using game;

namespace vom
{
    public class MapSystem : Ticker
    {
        public static MapSystem instance { get; private set; }

        public string testBaseMapId;
        public GameObject player;
        public Transform mapParent;
        public Transform tilesParent;

        public MapItem currentMap { get; private set; }
        List<MapItem> loadedMaps;

        public int tileNumRight;
        public int tileNumForward;
        public int tileNumBackward;

        private Vector2Int _lastPos;
        private Dictionary<Vector2Int, MapTileBehaviour> _tiles;
        public int gen { get; private set; }
        public bool paused;

        public string prefabId = "MapTile";
        public GameObject gameCoreGameObjects;

        int _globalOffsetX;
        int _globalOffsetZ;

        private struct MapTileData
        {
            public TileCacheBehaviour.OutputTileData tileData;
            public MapItem map;

            public MapTileData(TileCacheBehaviour.OutputTileData pTileData, MapItem pMap)
            {
                this.tileData = pTileData;
                this.map = pMap;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            InitMapSynchronizer();
            StartMap();//TODO call this after load from saved data
        }

        public void StartMap()
        {
            GenerateStartingMap(MapService.GetMapItemById(testBaseMapId));
            gameCoreGameObjects.SetActive(true);
        }

        void GenerateStartingMap(MapItem mapItem)
        {
            var map = Instantiate<MapItem>(mapItem, mapParent);
            var playerPos = player.transform.position;
            var mapPos = map.transform.position;

            //PlacePlayer on the right pos;
            var playerStartPos = map.playerStart.position;
            mapPos += playerPos - playerStartPos;
            mapPos.y = 0;
            map.transform.position = mapPos;

            currentMap = map;
            map.offsetX = Mathf.FloorToInt(mapPos.x);
            map.offsetZ = Mathf.FloorToInt(mapPos.z);
            _globalOffsetX = map.offsetX;
            _globalOffsetZ = map.offsetZ;

            loadedMaps.Add(map);
            mapFeedbackSystem.EnterNewMap();
        }

        void InitMapSynchronizer()
        {
            loadedMaps = new List<MapItem>();
            _lastPos = Vector2Int.zero;
            _tiles = new Dictionary<Vector2Int, MapTileBehaviour>();

            Clear();
        }

        protected override void Tick()
        {
            if (paused)
                return;

            Sync();
        }

        public void Clear()
        {
            gen = 0;
            foreach (var tile in _tiles)
            {
                if (tile.Value != null && tile.Value.gameObject != null)
                {
                    PoolingService.instance.Recycle(tile.Value.gameObject);
                }
            }
            _tiles = new Dictionary<Vector2Int, MapTileBehaviour>();
        }

        Vector2Int GetPlayerRelativeIntPos()
        {
            var pos = PlayerBehaviour.instance.transform.position + currentMap.playerStart.localPosition;
            return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.z));
        }

        MapTileData GetMapTileData(int x, int z)
        {
            TryGenerateConnectedMap(x, z);

            foreach (var loadedMap in loadedMaps)
            {
                foreach (var t in loadedMap.tiles)
                {
                    if (t.x + loadedMap.offsetX - _globalOffsetX == x && t.z + loadedMap.offsetZ - _globalOffsetZ == z)
                    {
                        //Debug.Log("loadedMap xz " + x + " " + z);
                        //Debug.Log("MapTileData from other " + t.x + " " + t.z + " map " + loadedMap.mapId);
                        return new MapTileData(t, loadedMap);
                    }
                }
            }

            return new MapTileData(new TileCacheBehaviour.OutputTileData(), null);
        }

        void TryGenerateConnectedMap(int x, int z)
        {
            //Debug.Log("TryGenerateConnectedMap " + x + " " + z);
            //Debug.Log(currentMap.sizeX);
            //Debug.Log(currentMap.sizeZ);
            foreach (var c in currentMap.connectors)
            {
                if ((x >= currentMap.sizeX && c.type == MapConnectorPrototype.ConnectToType.Right) ||
                    (x <= -1 && c.type == MapConnectorPrototype.ConnectToType.Left) ||
                    (z >= currentMap.sizeZ && c.type == MapConnectorPrototype.ConnectToType.Forward) ||
                    (z <= -1 && c.type == MapConnectorPrototype.ConnectToType.Backward))
                {
                    LoadConnectedMap(x, z, c, currentMap);
                }
            }
        }

        void LoadConnectedMap(int x, int z, MapConnectorPrototype connector, MapItem fromMap)
        {
            var toMapId = connector.toId;
            foreach (var loadedMap in loadedMaps)
            {
                if (loadedMap.mapId == toMapId)
                {
                    //Debug.Log("loaded " + mapId);
                    return;
                }
            }

            Debug.LogWarning("LoadConnectedMap " + toMapId);
            var toMapPrefab = MapService.GetMapItemById(toMapId);

            MapConnectorPrototype prefabToMapConnector = null;
            foreach (var pConnector in toMapPrefab.connectors)
            {
                //must ensure there is only 1 connector between 2 maps!
                if (pConnector.toId == connector.fromId)
                {
                    prefabToMapConnector = pConnector;
                    break;
                }
            }

            if (prefabToMapConnector == null)
            {
                Debug.LogError("no loadedMapConnector " + toMapId);
                return;
            }

            var toMap = Instantiate<MapItem>(toMapPrefab, mapParent);

            MapConnectorPrototype toMapConnector = null;
            foreach (var pConnector in toMap.connectors)
            {
                if (pConnector.toId == connector.fromId)
                {
                    toMapConnector = pConnector;
                    break;
                }
            }

            Vector2 toMapOffset = Vector2.zero;
            switch (connector.type)
            {
                case MapConnectorPrototype.ConnectToType.Backward:
                    toMapOffset.x = fromMap.transform.position.x;
                    toMapOffset.y = fromMap.transform.position.z - toMap.sizeZ;
                    toMapOffset.x += Mathf.Floor(connector.posPercentage * fromMap.sizeX) + connector.posOffset;
                    toMapOffset.x -= Mathf.Floor(toMapConnector.posPercentage * toMap.sizeX) + toMapConnector.posOffset;
                    break;

                case MapConnectorPrototype.ConnectToType.Forward:
                    toMapOffset.x = fromMap.transform.position.x;
                    toMapOffset.y = fromMap.transform.position.z + fromMap.sizeZ;
                    toMapOffset.x += Mathf.Floor(connector.posPercentage * fromMap.sizeX) + connector.posOffset;
                    toMapOffset.x -= Mathf.Floor(toMapConnector.posPercentage * toMap.sizeX) + toMapConnector.posOffset;
                    break;
                case MapConnectorPrototype.ConnectToType.Left:
                    toMapOffset.x = fromMap.transform.position.x - toMap.sizeX;
                    toMapOffset.y = fromMap.transform.position.z;
                    toMapOffset.y += Mathf.Floor(connector.posPercentage * fromMap.sizeZ) + connector.posOffset;
                    toMapOffset.y -= Mathf.Floor(toMapConnector.posPercentage * toMap.sizeZ) + toMapConnector.posOffset;
                    break;

                case MapConnectorPrototype.ConnectToType.Right:
                    toMapOffset.x = fromMap.transform.position.x + fromMap.sizeX;
                    toMapOffset.y = fromMap.transform.position.z;
                    toMapOffset.y += Mathf.Floor(connector.posPercentage * fromMap.sizeZ) + connector.posOffset;
                    toMapOffset.y -= Mathf.Floor(toMapConnector.posPercentage * toMap.sizeZ) + toMapConnector.posOffset;
                    break;
            }

            Debug.Log("toMapOffset " + toMapOffset);
            //this rect pos of starting map is 
            //Pos  -playerstart.x, -playerstart.z(BL)
            // BL -playerstart.x, -playerstart.z
            // BR sizeX-playerstart.x, -playerstart.z
            // TL -playerstart.x, sizeZ-playerstart.z
            // TR sizeX-playerstart.x, sizeZ-playerstart.z
            Vector3 toMapPos = new Vector3(toMapOffset.x, 0, toMapOffset.y);

            toMap.offsetX = Mathf.FloorToInt(toMapPos.x);
            toMap.offsetZ = Mathf.FloorToInt(toMapPos.z);

            toMap.transform.position = toMapPos;

            loadedMaps.Add(toMap);
        }

        void Sync()
        {
            gen++;
            Vector2Int playerIntPos = GetPlayerRelativeIntPos();

            if (gen > 1 && playerIntPos == _lastPos)
                return;

            _lastPos = playerIntPos;
            for (int x = playerIntPos.x - tileNumRight; x <= playerIntPos.x + tileNumRight; x++)
            {
                for (int y = playerIntPos.y - tileNumBackward; y <= playerIntPos.y + tileNumForward; y++)
                {
                    Vector2Int pPos = new Vector2Int(x, y);
                    if (_tiles.ContainsKey(pPos) && _tiles[pPos] != null && _tiles[pPos].gameObject != null && _tiles[pPos].gameObject.activeSelf)
                    {
                        _tiles[pPos].gen = gen;
                    }
                    else
                    {
                        var mapTileData = GetMapTileData(x, y);

                        var go = PoolingService.instance.GetInstance(prefabId);
                        var tile = go.GetComponent<MapTileBehaviour>();

                        go.transform.SetParent(tilesParent);
                        tile.gen = gen;
                        if (mapTileData.map != null)
                        {
                            tile.tileData = mapTileData.tileData;
                            tile.offset = new Vector2(mapTileData.map.transform.position.x, mapTileData.map.transform.position.z);
                            tile.Visualize();
                        }

                        if (_tiles.ContainsKey(pPos))
                            _tiles[pPos] = tile;
                        else
                            _tiles.Add(pPos, tile);
                    }
                }
            }

            foreach (var tile in _tiles)
            {
                if (tile.Value == null || tile.Value.gameObject == null || !tile.Value.gameObject.activeSelf)
                    continue;

                if (tile.Value.gen != gen)
                {
                    PoolingService.instance.Recycle(tile.Value.gameObject);
                }
                else
                {
                    tile.Value.SyncPos();
                }
            }
        }

        public MapFeedbackSystem mapFeedbackSystem;
    }
}