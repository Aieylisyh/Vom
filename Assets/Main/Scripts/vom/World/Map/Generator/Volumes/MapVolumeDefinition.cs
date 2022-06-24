using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public enum VolumeCoreType
    {
        Normal = 0,
        Void = 10,
        Fixed = 20,
    }

    public enum VolumeTerrainType
    {
        Normal = 0,
        Low = 20,
        High = 30,
        Wall = 50,
    }

    public enum VolumeGroundType
    {
        Normal = 0,
        Better = 10,
        Special = 20,
        VerySpecial = 30,
        Water = 40,//lower
    }

    public enum VolumeObstacleType
    {
        None = 0,
        Tree = 10,
        Stone = 20,
        Vase = 30,
        Trap = 50,
    }

    [System.Serializable]
    public struct VolumeSetter
    {
        public VolumeCoreType core;
        public VolumeTerrainType terrain;
        public VolumeGroundType ground;
        public int groundInfluenceTileDistance;
        public VolumeObstacleType obstacle;
        public int obstaclePercentage;
    }

    [System.Serializable]
    public struct VolumeTerrainDefinition
    {
        public VolumeTerrainType type;
        public float high;
        public float low;
    }

    [System.Serializable]
    public struct VolumeGroundDefinition
    {
        public Biome biome;
        public VolumeGroundType type;
        public List<GameObject> grounds;
    }

    [System.Serializable]
    public struct VolumeObstacleDefinition
    {
        public Biome biome;
        public VolumeObstacleType type;
        public List<GameObject> obstacles;
    }
}