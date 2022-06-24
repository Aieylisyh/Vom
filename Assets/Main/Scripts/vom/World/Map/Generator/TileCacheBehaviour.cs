using UnityEngine;
using System;

namespace vom
{
    public class TileCacheBehaviour : MonoBehaviour
    {
        public VolumeSetter volume;

        public OutputTileData tileData;

        public MapPrototype mapPrototype;

        public bool toggleShow;

        public GameObject groundView;
        public GameObject obstacleView;

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

            //volume = new VolumeSetter();
            //volume.obstaclePercentage = 100;
            //volume.core = VolumeCoreType.Normal;
            //volume.ground = VolumeGroundType.Normal;
            //volume.obstacle = VolumeObstacleType.None;
            //volume.terrain = VolumeTerrainType.Normal;
            //volume.groundInfluenceTileDistance = 0;
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
            tileData.tile = MapGeneratorSystem.instance.mapVolumeSystem.GetObstacle(this.volume, mapPrototype.biome);
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

            if (groundView == null)
            {
                if (tileData.tile != null)
                {
                    CreateGroundView();
                }
            }
            else
            {
                Destroy(groundView);
                groundView = null;
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

            if (obstacleView == null)
            {
                if (tileData.tile != null)
                {
                    CreateObstacleView();
                }
            }
            else
            {
                Destroy(obstacleView);
                obstacleView = null;
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
        }
    }
}