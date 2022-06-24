using UnityEngine;

namespace vom
{
    public class TileBehaviour : MonoBehaviour
    {
        public Vector2Int pos;
        public int gen;
        public float height = 0f;

        public void SyncPos()
        {
            transform.position = new Vector3(pos.x, height, pos.y);
        }
    }
}