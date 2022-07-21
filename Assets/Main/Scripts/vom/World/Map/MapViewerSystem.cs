using UnityEngine;

namespace vom
{
    public class MapViewerSystem : MonoBehaviour
    {
        public MapItem item;
        public Transform tilesParent;
        public SimpleTileCacheBehaviour tileViewerCache;

        public static MapViewerSystem instance;

        void Start()
        {
            instance = this;

            foreach (var tile in item.tiles)
            {
                var tileCache = Instantiate(tileViewerCache, new Vector3(tile.x, 0, tile.z), Quaternion.identity, tilesParent);
                tileCache.tileData = tile;
                tileCache.mapPrototype = item.prototype;
                tileCache.Visualize();
                tileCache.indexOfMapitem = item.tiles.IndexOf(tile);//will not return duplicate index!
            }
        }

        public void SyncMapItem(SimpleTileCacheBehaviour cache)
        {
            item.tiles[cache.indexOfMapitem] = cache.tileData;
        }
    }
}
