using UnityEngine;
using System;

namespace vom
{
    public class SimpleTileCacheBehaviour : MonoBehaviour
    {
        public TileCacheBehaviour.OutputTileData tileData;
        public int indexOfMapitem;

        public MapPrototype mapPrototype;

        public GameObject groundView;
        public GameObject obstacleView;

        public bool toggleSave;

        public void Init(int x, int z, MapPrototype p)
        {
            tileData.x = x;
            tileData.z = z;
            mapPrototype = p;
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
            obstacleView.transform.localPosition = new Vector3(0, tileData.h * 0.01f, 0);
        }

        void CreateGroundView()
        {
            groundView = Instantiate(tileData.tile, transform);
            groundView.transform.localPosition = new Vector3(0, tileData.h * 0.01f, 0);
        }

        private void Update()
        {
            if (toggleSave)
            {
                toggleSave = false;
                Visualize();
                MapViewerSystem.instance.SyncMapItem(this);
            }
        }
    }
}