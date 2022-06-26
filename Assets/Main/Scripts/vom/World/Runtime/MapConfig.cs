using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class MapConfig : ScriptableObject
    {
        public TilesParam tiles;

        public List<MapItem> maps;
    }

    [System.Serializable]
    public struct TilesParam
    {
        public List<Mesh> tileMeshes;

        public Mesh GetRandomTileMesh()
        {
            return tileMeshes[Random.Range(0, tileMeshes.Count)];
        }
    }
}