using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class MapItem : MonoBehaviour
    {
        public List<TileCacheBehaviour.OutputTileData> tiles;
        public MapPrototype prototype;
        public Transform playerStart;
        public BlessingBehaviour blessing;
    }
}