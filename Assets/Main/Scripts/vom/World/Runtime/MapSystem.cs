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

        public int tileNumRight;
        public int tileNumForward;
        public int tileNumBackward;

        private Vector2Int _lastPos;
        private Dictionary<Vector2Int, MapTileBehaviour> _tiles;
        public int gen { get; private set; }
        public bool paused;

        public string prefabId = "MapTile";
        public GameObject gameCoreGameObjects;

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

            var playerStartPos = map.playerStart.position;
            //PlacePlayer on the right pos;
            mapPos += playerPos - playerStartPos;
            mapPos.y = 0;
            map.transform.position = mapPos;

            currentMap = map;
            mapFeedbackSystem.EnterNewMap();
        }

        void InitMapSynchronizer()
        {
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
            return new Vector2Int((int)pos.x, (int)pos.z);
        }

        TileCacheBehaviour.OutputTileData GetTileData(int x, int z)
        {
            foreach (var t in currentMap.tiles)
            {
                if (t.x == x && t.z == z)
                    return t;
            }

            return TryFindOtherTileData(x, z);
        }

        TileCacheBehaviour.OutputTileData TryFindOtherTileData(int x, int z)
        {
            Debug.LogWarning("no data x" + x + " z" + z);
            TryGenerateConnectedMap(x, z);

            return new TileCacheBehaviour.OutputTileData();
        }

        void TryGenerateConnectedMap(int x, int z)
        {
            Debug.Log("TryGenerateConnectedMap " + x + " " + z);
            //Debug.Log(currentMap.sizeX);
            //Debug.Log(currentMap.sizeZ);
            foreach (var c in currentMap.connectors)
            {
                if (x >= currentMap.sizeX && c.type == MapConnectorPrototype.ConnectToType.Right)
                {
                    Debug.Log(c.toId);
                }
                else if (x <= -1 && c.type == MapConnectorPrototype.ConnectToType.Left)
                {
                    Debug.Log(c.toId);
                }
                else if (z >= currentMap.sizeZ && c.type == MapConnectorPrototype.ConnectToType.Forward)
                {
                    Debug.Log(c.toId);
                }
                else if (z <= -1 && c.type == MapConnectorPrototype.ConnectToType.Backward)
                {
                    Debug.Log(c.toId);
                }
            }
            return;
            var mapItem = MapService.GetMapItemById(testBaseMapId);
            var map = Instantiate<MapItem>(mapItem, mapParent);
            var playerPos = player.transform.position;
            var mapPos = map.transform.position;

            var playerStartPos = currentMap.playerStart.position;
            //PlacePlayer on the right pos;
            mapPos += playerPos - playerStartPos;
            mapPos.y = 0;
            map.transform.position = mapPos;

            currentMap = map;
            mapFeedbackSystem.EnterNewMap();
        }

        void Sync()
        {
            gen++;
            Vector2Int intPos = GetPlayerRelativeIntPos();

            if (gen > 1 && intPos == _lastPos)
                return;

            _lastPos = intPos;
            for (int x = intPos.x - tileNumRight; x <= intPos.x + tileNumRight; x++)
            {
                for (int y = intPos.y - tileNumBackward; y <= intPos.y + tileNumForward; y++)
                {
                    Vector2Int pPos = new Vector2Int(x, y);
                    if (_tiles.ContainsKey(pPos) && _tiles[pPos] != null && _tiles[pPos].gameObject != null && _tiles[pPos].gameObject.activeSelf)
                    {
                        _tiles[pPos].gen = gen;
                    }
                    else
                    {
                        var tileData = GetTileData(x, y);
                        var go = PoolingService.instance.GetInstance(prefabId);

                        var tile = go.GetComponent<MapTileBehaviour>();
                        if (tile == null)
                        {
                            Debug.LogError("no MapTileBehaviour");
                        }

                        go.transform.SetParent(tilesParent);
                        tile.tileData = tileData;
                        tile.gen = gen;
                        tile.offset = new Vector2Int(-(int)currentMap.playerStart.localPosition.x, -(int)currentMap.playerStart.localPosition.z);
                        tile.Visualize();

                        if (_tiles.ContainsKey(pPos))
                        {
                            _tiles[pPos] = tile;
                        }
                        else
                        {
                            _tiles.Add(pPos, tile);
                        }
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