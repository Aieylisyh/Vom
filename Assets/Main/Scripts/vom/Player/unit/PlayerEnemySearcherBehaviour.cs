using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class PlayerEnemySearcherBehaviour : MonoBehaviour
    {
        public AttackRange range;
        float _fRange;

        public int maskToInclude;

        private void Start()
        {
            _fRange = CombatSystem.GetRange(range);
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

        public bool BlockedByTile(EnemyBehaviour e, float range)
        {
            if (range < 1)
                return false;

            var pos = transform.position;
            var dir = e.transform.position - pos;

            RaycastHit[] hitInfos = Physics.RaycastAll(pos, dir, range, 1 << maskToInclude);
            if (hitInfos != null && hitInfos.Length > 0)
            {
                foreach (var hitInfo in hitInfos)
                {
                    var hitObj = hitInfo.collider.gameObject;
                    //Debug.Log("ene " + e.gameObject + " blocked by " + hitObj);
                    if (hitObj != null && hitObj.tag == "Ground")
                    {
                        //Debug.Log(e.gameObject + " blocked tile " + hitObj);
                        //Debug.Log(hitObj.transform.parent.localPosition);
                        return true;
                    }
                }
            }

            return false;
        }

        public EnemyBehaviour GetTargetEnemy()
        {
            var enemies = EnemySystem.instance.enemies;

            var pos = transform.position;
            var minDist = float.MaxValue;
            EnemyBehaviour nearestEnemy = null;

            foreach (var e in enemies)
            {
                if (e.death.dead)
                {
                    continue;
                }
                var dir = e.transform.position - pos;
                if (dir.magnitude >= _fRange)
                {
                    //Debug.Log("ene " + e.gameObject + " too far");
                    continue;
                }

                if (BlockedByTile(e, dir.magnitude))
                    continue;

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