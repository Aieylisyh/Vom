using UnityEngine;
using System.Collections.Generic;
using game;
using com;

namespace vom
{
    public class TileGenerateSystem : Ticker
    {
        public static TileGenerateSystem instance { get; private set; }

        public int tileNumRight;
        public int tileNumForward;
        public int tileNumBackward;

        private Vector2Int _lastPos;

        private Dictionary<Vector2Int, TileBehaviour> _tiles;

        public GameObject tilePrefab;

        public int gen { get; private set; }

        public bool paused;

        public Transform spawnParent;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            _lastPos = Vector2Int.zero;
            _tiles = new Dictionary<Vector2Int, TileBehaviour>();

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
            _tiles = new Dictionary<Vector2Int, TileBehaviour>();
        }

        void Sync()
        {
            gen++;
            var pos = PlayerBehaviour.instance.transform.position;
            Vector2Int intPos = new Vector2Int((int)pos.x, (int)pos.z);

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
                        var go = Instantiate(tilePrefab);
                        go.SetActive(true);
                        var tile = go.GetComponent<TileBehaviour>();
                        if (tile == null)
                        {
                            Debug.LogError("no TileBehaviour in tile");
                        }
                        go.transform.SetParent(spawnParent);
                        tile.pos = pPos;
                        tile.gen = gen;
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