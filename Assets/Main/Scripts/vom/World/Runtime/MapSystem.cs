using UnityEngine;
using System.Collections.Generic;
using com;
using game;

namespace vom
{
    public class MapSystem : Ticker
    {
        public MapItem item;
        public GameObject player;
        public Transform mapParent;
        public Transform tilesParent;

        public MapItem currentMap;

        private void Start()
        {
            GeneratorMap();
            InitMap();
        }

        void GeneratorMap()
        {
            //PlacePlayer on the right pos;
            currentMap = Instantiate<MapItem>(item, mapParent);
            var pos = player.transform.position;
            var playerStartPos = currentMap.playerStart.position;

            currentMap.transform.position = currentMap.transform.position + pos - playerStartPos;
        }

        public int tileNumRight;
        public int tileNumForward;
        public int tileNumBackward;

        private Vector2Int _lastPos;
        private Dictionary<Vector2Int, MapTileBehaviour> _tiles;
        public int gen { get; private set; }
        public bool paused;

        public string prefabId = "MapTile";

        void InitMap()
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
                {
                    return t;
                }
            }

            Debug.LogWarning("no data x" + x + " z" + z);

            return new TileCacheBehaviour.OutputTileData();
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
                        var tileData = GetTileData(intPos.x, intPos.y);
                        var go = PoolingService.instance.GetInstance(prefabId);

                        var tile = go.GetComponent<MapTileBehaviour>();
                        if (tile == null)
                        {
                            Debug.LogError("no MapTileBehaviour");
                        }

                        go.transform.SetParent(tilesParent);
                        tile.tileData = tileData;
                        tile.gen = gen;
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
    }
}