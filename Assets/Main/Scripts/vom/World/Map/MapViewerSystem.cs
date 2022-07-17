using UnityEngine;
using System.Collections;

namespace vom
{
    public class MapViewerSystem : MonoBehaviour
    {
        public MapItem item;
        public Transform tilesParent;

        void Start()
        {
            foreach (var tile in item.tiles)
            {
                Visualize(tile);
            }
        }


        public void Visualize(TileCacheBehaviour.OutputTileData tileData)
        {
            bool hasGround = (tileData.tile != null);
            if (hasGround)
            {
                CreateGroundView(tileData);
            }

            bool hasObstacle = (tileData.obstacle != null);
            if (hasObstacle)
            {
                CreateObstacleView(tileData);
            }
        }

        void CreateObstacleView(TileCacheBehaviour.OutputTileData tileData)
        {
            var obstacleView = Instantiate(tileData.obstacle, tilesParent);
            obstacleView.transform.localPosition = new Vector3(tileData.x, tileData.h * 0.01f, tileData.z);
        }

        void CreateGroundView(TileCacheBehaviour.OutputTileData tileData)
        {
            var groundView = Instantiate(tileData.tile, tilesParent);
            groundView.transform.localPosition = new Vector3(tileData.x, tileData.h*0.01f, tileData.z);
        }
    }
}
