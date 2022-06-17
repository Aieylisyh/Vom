using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace vom
{
    public class PlayerOrbBehaviour : MonoBehaviour
    {
        public static PlayerOrbBehaviour instance { get; private set; }
        public Transform spawnSpace;

        public GameObject fireball;
        private List<OrbBehaviour> _orbs = new List<OrbBehaviour>();

        private void Awake()
        {
            instance = this;
        }

        public void TestFireBalls()
        {
            SpawnOrb(fireball, 0);
            SpawnOrb(fireball, 120);
            SpawnOrb(fireball, 240);
        }

        public void TestArcane()
        {

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

        public void ReleaseFirst(GameObject target)
        {
            ClearInvalids();

            if (_orbs.Count > 0)
            {
                _orbs[0].SetRelease(target.transform);
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