using UnityEngine;

namespace vom
{
    public class EnemySpawnBehaviour : game.Ticker
    {
        public GameObject prefab;
        bool spawned;

        protected override void Tick()
        {
            if (IsInPlayerView())
            {
                Instantiate(prefab, transform.position, transform.rotation, transform.parent);
                Destroy(gameObject);
            }
        }

        public bool IsInPlayerView()
        {
            var playerPos = PlayerBehaviour.instance.transform.position;
            var toPlayerDir = playerPos - transform.position;

            var dX = toPlayerDir.x;
            var dZ = toPlayerDir.z;
            //Debug.Log(dX + " " + dZ + " | " + MapSystem.instance.tileNumRight + " " + MapSystem.instance.tileNumForward + " " + MapSystem.instance.tileNumBackward);
            if (Mathf.Abs(dX) < MapSystem.instance.tileNumRight)
            {
                if (-dZ < MapSystem.instance.tileNumForward && dZ < MapSystem.instance.tileNumBackward)
                {
                    return true;
                }
            }

            return false;
        }
    }
}