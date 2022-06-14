using UnityEngine;

namespace game
{
    public class EnemyMuzzle : MonoBehaviour
    {
        void Start()
        {
            var ea = GetComponentInParent<EnemyAttack>();

            if (ea == null)
                return;
            ea.muzzlePos = transform;
        }
    }
}
