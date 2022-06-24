using UnityEngine;
using System;

namespace vom
{
    public class TileCacheBehaviour : MonoBehaviour
    {
        public VolumeSetter volume;

        public OutputTileData tileData;

        public MapPrototype mapPrototype;

        public GameObject groundView;
        public GameObject obstacleView;

        public bool toggleBuild;
        public bool toggleShow;

        [Serializable]
        public struct OutputTileData
        {
            public GameObject tile;
            public GameObject obstacle;
            public int x;
            public int z;
            public float height;
        }

        public void Init(int x, int z, MapPrototype p)
        {
            tileData.x = x;
            tileData.z = z;
            mapPrototype = p;

            volume.core = VolumeCoreType.Normal;

            volume.terrain = VolumeTerrainType.Normal;

            volume.ground = VolumeGroundType.Normal;
            volume.groundPercentage = 100;

            volume.obstacle = VolumeObstacleType.None;
            volume.obstaclePercentage = 100;
        }

        public void Build()
        {
            if (volume.core == VolumeCoreType.Void)
            {
                tileData.tile = null;
                tileData.obstacle = null;
                tileData.height = 0;
                return;
            }

            if (volume.core == VolumeCoreType.Fixed)
            {
                return;
            }

            tileData.height = MapVolumeService.GetHeight(this);
            tileData.tile = MapGeneratorSystem.instance.mapVolumeSystem.GetGround(this.volume, mapPrototype.biome);

            if (volume.ground != VolumeGroundType.Water)
            {
                //(volume.terrain != VolumeTerrainType.Wall ??
                tileData.obstacle = MapGeneratorSystem.instance.mapVolumeSystem.GetObstacle(this.volume, mapPrototype.biome);
            }
            else
            {
                tileData.obstacle = null;
            }
        }

        public void Visualize()
        {
            bool hasGround = (tileData.tile != null);
            if (groundView == null && hasGround)
            {
                CreateGroundView();
            }
            else if (groundView == null && !hasGround)
            {
                //nothing
            }
            else if (groundView != null && !hasGround)
            {
                Destroy(groundView);
                groundView = null;
            }
            else if (groundView != null && hasGround)
            {
                Destroy(groundView);
                CreateGroundView();
            }

            bool hasObstacle = (tileData.obstacle != null);
            if (obstacleView == null && hasObstacle)
            {
                CreateObstacleView();
            }
            else if (obstacleView == null && !hasObstacle)
            {
                //nothing
            }
            else if (obstacleView != null && !hasObstacle)
            {
                Destroy(obstacleView);
                obstacleView = null;
            }
            else if (obstacleView != null && hasObstacle)
            {
                Destroy(obstacleView);
                CreateObstacleView();
            }
        }

        void CreateObstacleView()
        {
            obstacleView = Instantiate(tileData.obstacle, transform);
            obstacleView.transform.localPosition = new Vector3(0, tileData.height, 0);
        }

        void CreateGroundView()
        {
            groundView = Instantiate(tileData.tile, transform);
            groundView.transform.localPosition = new Vector3(0, tileData.height, 0);
        }

        private void Update()
        {
            if (toggleShow)
            {
                toggleShow = false;
                Visualize();
            }

            if (toggleBuild)
            {
                toggleBuild = false;
                Build();
                toggleShow = true;
            }
        }
    }
}