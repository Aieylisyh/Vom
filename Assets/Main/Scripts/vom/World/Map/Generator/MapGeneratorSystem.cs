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

        public MapVolumeSystem mapVolumeSystem { get; private set; }
        public GameObject tileCachePrefab;
        public Transform tilesParent;

        public List<TileCacheBehaviour> tiles { get; private set; }

        public bool willVisualize = false;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mapVolumeSystem = GetComponent<MapVolumeSystem>();
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

            Invoke("GenerateStep2", 2.0f);
        }

        void GenerateStep2()
        {
            Debug.Log("GenerateStep2");
            foreach (var t in tiles)
            {
                t.Build();
            }

            if (willVisualize)
            {
                Invoke("GenerateStep3", 2.0f);
            }
        }

        void GenerateStep3()
        {
            Debug.Log("GenerateStep3");
            foreach (var t in tiles)
            {
                t.Visualize();
            }
        }
    }
}