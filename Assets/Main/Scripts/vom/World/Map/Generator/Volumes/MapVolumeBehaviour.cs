using UnityEngine;

namespace vom
{
    public class MapVolumeBehaviour : MonoBehaviour
    {
        public VolumeSetter volume;
        public TileCacheBehaviour.OutputTileData tileData;

        private void OnTriggerEnter(Collider other)
        {
            var tile = other.GetComponent<TileCacheBehaviour>();
            if (tile != null)
            {
                tile.volume = MapVolumeTypeMerger.Merge(volume, tile.volume);

                if (volume.core == VolumeCoreType.Fixed)
                {
                    tile.tileData.tile = tileData.tile;
                    tile.tileData.obstacle = tileData.obstacle;
                    tile.tileData.height = tileData.height;
                }
            }
        }
    }
}
