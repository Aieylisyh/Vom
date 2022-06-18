using UnityEngine;
using com;

namespace vom
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public void OnHit(OrbBehaviour orb)
        {
            CameraShake.instance.Shake(orb.hitShakeLevel);
        }
    }
}