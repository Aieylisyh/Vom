using UnityEngine;

namespace vom
{
    public class MapTileBehaviour : MonoBehaviour
    {
        public int gen;

        public GameObject groundView;
        public GameObject obstacleView;
        public TileCacheBehaviour.OutputTileData tileData;
        public Vector2Int offset;

        public void SyncPos()
        {
            transform.position = new Vector3(tileData.x + offset.x, tileData.height, tileData.z + offset.y);
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
            obstacleView.transform.localPosition = new Vector3(0, 0, 0);
        }

        void CreateGroundView()
        {
            groundView = Instantiate(tileData.tile, transform);
            groundView.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}