using UnityEngine;
using System.Collections.Generic;


namespace vom
{
    public class MapGeneratorSystem : MonoBehaviour
    {
        public static MapGeneratorSystem instance;

        public MapPrototype prototype;

        public int size_x = 60;
        public int size_z = 100;

        public MapVolumeSystem mapVolumeSystem;
        public GameObject tileCachePrefab;
        public Transform tilesParent;

        public List<TileCacheBehaviour> tiles { get; private set; }

        public bool willVisualize = false;
        public MapItem mapItem;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //mapVolumeSystem = GetComponent<MapVolumeSystem>();
            tiles = new List<TileCacheBehaviour>();

            Invoke("GenerateStep1", 0.1f);
        }

        void GenerateStep1()
        {
            Debug.Log("GenerateStep1");
            for (int x = 0; x < size_x; x++)
            {
                for (int z = 0; z < size_z; z++)
                {
                    var t = Instantiate(tileCachePrefab, new Vector3(x, 0, z), Quaternion.identity, tilesParent);
                    var tg = t.GetComponent<TileCacheBehaviour>();
                    tg.Init(x, z, prototype);
                    tiles.Add(tg);
                }
            }

            Invoke("GenerateStep2", 1.0f);
        }

        void GenerateStep2()
        {
            Debug.Log("GenerateStep2");
            foreach (var t in tiles)
            {
                t.Build();
            }

            Invoke("GenerateStep3", 1.0f);
        }

        void GenerateStep3()
        {
            Debug.Log("GenerateStep3");
            if (willVisualize)
            {
                foreach (var t in tiles)
                {
                    t.Visualize();
                }
            }
        }

        void SaveToItem()
        {
            Debug.Log("SaveToItem");

            mapItem.tiles = new List<TileCacheBehaviour.OutputTileData>();
            foreach (var tile in tiles)
            {
                mapItem.tiles.Add(tile.tileData);
            }

            mapItem.prototype = prototype;
        }

        public bool toggleSave;
        private void Update()
        {
            if (toggleSave)
            {
                toggleSave = false;
                SaveToItem();
            }
        }
    }
}