using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class OrbsController : VomPlayerComponent
    {
        public GameObject fireball;
        public GameObject iceball;
        public GameObject poisonball;

        public GameObject arcaneBolt;


        private List<OrbBehaviour> _orbs = new List<OrbBehaviour>();
        public Transform weaponPos;

        public void AddFireBalls()
        {
            Clear();
            SpawnOrb(fireball, 0);
            SpawnOrb(fireball, 120);
            SpawnOrb(fireball, 240);
        }

        public void AddIceBalls()
        {
            Clear();
            SpawnOrb(iceball, 40);
            SpawnOrb(iceball, 220);
        }

        public void AddPoisonBalls()
        {
            Clear();
            SpawnOrb(poisonball, 20);
            SpawnOrb(poisonball, 110);
            SpawnOrb(poisonball, 200);
            SpawnOrb(poisonball, 290);
        }

        public void LaunchArcane(Vector3 targetPos)
        {
            SpawnShoot(arcaneBolt, targetPos);
        }

        void SpawnOrb(GameObject prefab, float degree)
        {
            GameObject orbGo = Instantiate(prefab, CombatSystem.instance.projectileSpace);
            orbGo.SetActive(true);
            var orb = orbGo.GetComponent<OrbBehaviour>();
            orb.SetOrbital(degree, PlayerBehaviour.instance.transform);
            _orbs.Add(orb);
        }

        void SpawnShoot(GameObject prefab, Vector3 targetPos)
        {
            GameObject shootGo = Instantiate(prefab, CombatSystem.instance.projectileSpace);
            shootGo.SetActive(true);

            var shoot = shootGo.GetComponent<OrbBehaviour>();
            shoot.SetOrigin(weaponPos.position, true);
            shoot.SetRelease(targetPos);
        }

        public void Clear()
        {
            for (var i = _orbs.Count - 1; i >= 0; i--)
            {
                var orb = _orbs[i];
                if (orb != null)
                {
                    Destroy(orb.gameObject);
                }
                _orbs.Remove(orb);
            }
        }

        public void ReleaseFirst(Vector3 target)
        {
            ClearInvalids();

            if (_orbs.Count > 0)
            {
                var orb = _orbs[0];
                if (orb.IsReadyInOrbital())
                {
                    orb.SetOrigin(orb.transform.position, false);
                    orb.SetRelease(target);
                    _orbs.Remove(orb);
                }
            }
        }

        void ClearInvalids()
        {
            for (var i = _orbs.Count - 1; i >= 0; i--)
            {
                var orb = _orbs[i];
                if (orb == null)
                {
                    _orbs.Remove(orb);
                }
            }
        }
    }
}