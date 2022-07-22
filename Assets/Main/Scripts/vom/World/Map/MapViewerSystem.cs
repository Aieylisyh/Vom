using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class MapViewerSystem : MonoBehaviour
    {
        public MapItem item;
        public Transform tilesParent;
        public SimpleTileCacheBehaviour tileViewerCache;
        public static MapViewerSystem instance;
        public List<SimpleTileCacheBehaviour> list;

        void Start()
        {
            instance = this;

            foreach (var tile in item.tiles)
            {
                SimpleTileCacheBehaviour tileCache = Instantiate(tileViewerCache, new Vector3(tile.x, 0, tile.z), Quaternion.identity, tilesParent);
                tileCache.tileData = tile;
                tileCache.mapPrototype = item.prototype;
                tileCache.Visualize();
                tileCache.indexOfMapitem = item.tiles.IndexOf(tile);//will not return duplicate index!
                list.Add(tileCache);
            }
        }

        public void SyncMapItem(SimpleTileCacheBehaviour cache)
        {
            item.tiles[cache.indexOfMapitem] = cache.tileData;
        }

        public void Duplicate(SimpleTileCacheBehaviour cache)
        {
            int index = cache.indexOfMapitem;
            int indexTarget = index + cache.duplicateOffsetX * item.sizeZ + cache.duplicateOffsetZ;
            //item.tiles[indexTarget] = cache.tileData;

            list[indexTarget].tileData = cache.tileData;
            list[indexTarget].toggleSave = true;
        }
    }
}