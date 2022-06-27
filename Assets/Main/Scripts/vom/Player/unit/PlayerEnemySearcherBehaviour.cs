using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class PlayerEnemySearcherBehaviour : MonoBehaviour
    {
        public AttackRange range = AttackRange.Melee;
        float _fRange;

        public int maskToInclude;

        private void Start()
        {
            _fRange = CombatSystem.instance.GetRange(range);
        }
        /*
        public void UpdateEnemies_nonBlockedFirst()
        {
            var enemies = EnemySystem.instance.enemies;

            RaycastHit hitInfo;
            //public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask);
            var pos = transform.position;
            var minDist = float.MaxValue;
            EnemyBehaviour nearestNonBlockedEnemy = null;

            foreach (var e in enemies)
            {
                var dir = e.transform.position - pos;
                if (dir.magnitude > _fRange)
                {
                    Debug.Log("ene " + e.gameObject + " too far");
                    continue;
                }
                var res = Physics.Raycast(pos, dir, out hitInfo, _fRange, 1 << maskToInclude);
                if (res) { Debug.Log("ene " + e.gameObject + " blocked by " + hitInfo.collider.gameObject); }
                else
                {
                    var dist = dir.magnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestNonBlockedEnemy = e;
                    }
                }
            }

            //mask https://blog.csdn.net/yongshuangzhao/article/details/102877973
            if (nearestNonBlockedEnemy != null)
            {
                Debug.Log("target is " + nearestNonBlockedEnemy.gameObject);
               // PlayerOrbBehaviour.instance.ReleaseFirst(nearestNonBlockedEnemy.gameObject);
            }
        }
        */
        public EnemyBehaviour GetTargetEnemy()
        {
            var enemies = EnemySystem.instance.enemies;

            var pos = transform.position;
            var minDist = float.MaxValue;
            EnemyBehaviour nearestEnemy = null;

            foreach (var e in enemies)
            {
                var dir = e.transform.position - pos;
                if (dir.magnitude > _fRange)
                {
                    //Debug.Log("ene " + e.gameObject + " too far");
                    continue;
                }

                var dist = dir.magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestEnemy = e;
                }
            }

            return nearestEnemy;
        }
    }
}