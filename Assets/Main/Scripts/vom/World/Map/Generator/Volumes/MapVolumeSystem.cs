using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class MapVolumeSystem : MonoBehaviour
    {
        public List<VolumeGroundDefinition> groundDefinitions;
        public List<VolumeTerrainDefinition> terrainDefinitions;
        public List<VolumeObstacleDefinition> obstacleDefinitions;

        public GameObject nullTile;
        public float waterLower = 0.15f;

        public GameObject GetGround(VolumeSetter vs, Biome biome)
        {
            var groundType = vs.ground;
            if (vs.terrain == VolumeTerrainType.Wall || 0.01f * vs.groundPercentage < Random.value)
                groundType = VolumeGroundType.Normal;

            foreach (var g in groundDefinitions)
            {
                if (g.biome == biome && g.type == groundType)
                {
                    if (g.grounds != null && g.grounds.Count > 0)
                        return g.grounds[Random.Range(0, g.grounds.Count)];

                    break;
                }
            }

            Debug.LogWarning("!nullTile");
            return nullTile;
        }

        public GameObject GetObstacle(VolumeSetter vs, Biome biome)
        {
            foreach (var g in obstacleDefinitions)
            {
                if (g.biome == biome && g.type == vs.obstacle)
                {
                    if (0.01f * vs.obstaclePercentage > Random.value)
                    {
                        if (g.obstacles != null && g.obstacles.Count > 0)
                            return g.obstacles[Random.Range(0, g.obstacles.Count)];
                    }

                    break;
                }
            }

            return null;
        }
    }
}