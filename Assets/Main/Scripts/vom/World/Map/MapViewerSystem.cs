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

            //  tileCache.indexOfMapitem = item.tiles.IndexOf(tile);//will not return duplicate index!
            int l = item.tiles.Count;
            for (int i = 0; i < l; i++)
            {
                var tile = item.tiles[i];
                int z = i % item.sizeZ;
                int x = (i - z) / item.sizeZ;
                SimpleTileCacheBehaviour tileCache = Instantiate(tileViewerCache, new Vector3(x, 0, z), Quaternion.identity, tilesParent);
                tileCache.tileData = tile;
                tileCache.mapPrototype = item.prototype;
                tileCache.Visualize();
                tileCache.indexOfMapitem = i;
                // tile.x = x;
                // tile.z = z;
                item.tiles[i] = tile;
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

            list[indexTarget].tileData.h = cache.tileData.h;
            list[indexTarget].tileData.obstacle = cache.tileData.obstacle;
            list[indexTarget].tileData.tile = cache.tileData.tile;
            list[indexTarget].toggleSave = true;
        }
    }
}