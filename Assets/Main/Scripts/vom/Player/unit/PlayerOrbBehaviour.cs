using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class PlayerOrbBehaviour : VomUnitComponent
    {
        public Transform spawnSpace;

        public GameObject fireball;
        private List<OrbBehaviour> _orbs = new List<OrbBehaviour>();

        public string spawnSound;

        public void AddFireBalls()
        {
            Clear();
            SpawnOrb(fireball, 0);
            SpawnOrb(fireball, 120);
            SpawnOrb(fireball, 240);
            com.SoundService.instance.Play(spawnSound);
        }

        public void LaunchArcane()
        {
            com.SoundService.instance.Play(spawnSound);
        }

        void SpawnOrb(GameObject prefab, float degree)
        {
            GameObject orbGo = Instantiate(prefab, spawnSpace);
            orbGo.SetActive(true);
            var orb = orbGo.GetComponent<OrbBehaviour>();
            orb.SetOrbital(degree, vom.PlayerBehaviour.instance.transform);
            _orbs.Add(orb);
        }

        void Clear()
        {
            if (_orbs.Count > 0)
            {
                if (_orbs[0] != null)
                {
                    Destroy(_orbs[0].gameObject);
                }
                _orbs.RemoveAt(0);
            }
        }

        public void ReleaseFirst(Vector3 target)
        {
            ClearInvalids();

            if (_orbs.Count > 0)
            {
                _orbs[0].SetRelease(target);
                _orbs.RemoveAt(0);
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