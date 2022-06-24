using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class MapVolumeService
    {
        static MapVolumeSystem system
        {
            get
            {
                return MapGeneratorSystem.instance.mapVolumeSystem;
            }
        }

        public static float GetHeight(TileCacheBehaviour t)
        {
            var terrainType = t.volume.terrain;
            float height = GetBaseHeight(t.volume.terrain, t.volume.ground);

            if (CanRebaseHeight(terrainType))
            {
                int adjacentSame = 0;
                int adjacentNonSame = 0;

                foreach (var tile in MapGeneratorSystem.instance.tiles)
                {
                    if (t != tile && (t.transform.position - tile.transform.position).magnitude < 1.1f)
                    {
                        if (tile.volume.core == VolumeCoreType.Normal && tile.volume.terrain == terrainType)
                            adjacentSame++;
                        else
                            adjacentNonSame++;
                    }
                }

                if (adjacentNonSame == 0)
                {
                    if (adjacentSame > 0)
                    {
                        height += GetBaseHeight(terrainType);
                    }
                }
            }

            return height;
        }

        public static float GetBaseHeight(VolumeTerrainType terrainType, VolumeGroundType groundType = VolumeGroundType.Normal)
        {
            float height = 0;
            foreach (var terrianDef in system.terrainDefinitions)
            {
                if (terrianDef.type == terrainType)
                {
                    height = Random.Range(terrianDef.low, terrianDef.high);
                    break;
                }
            }

            if (groundType == VolumeGroundType.Water)
                height -= system.waterLower;

            return height;
        }

        static bool CanRebaseHeight(VolumeTerrainType terrainType)
        {
            if (terrainType == VolumeTerrainType.Normal)
                return false;

            return true;
        }
    }
}