using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class VomDemoUiSystem : MonoBehaviour
    {
        public GameObject testTarget_standard;
        public PlayerBehaviour player;

        public GameObject orbPrefab;
        public Transform spawnSpace;

        private List<OrbBehaviour> _orbs = new List<OrbBehaviour>();

        private void Start()
        {
            GameFlowService.instance.SetInputState(GameFlowService.InputState.Allow);
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Gameplay);
        }

        public void OnClickFireBalls()
        {
            //charge 3 fire balls
            GameObject orb1Go = Instantiate(orbPrefab, spawnSpace);
            orb1Go.SetActive(true);
            var orb1 = orb1Go.GetComponent<OrbBehaviour>();
            orb1.SetOrbital(0, player.transform);
            _orbs.Add(orb1);

            GameObject orb2Go = Instantiate(orbPrefab, spawnSpace);
            orb2Go.SetActive(true);
            var orb2 = orb2Go.GetComponent<OrbBehaviour>();
            orb2.SetOrbital(120, player.transform);
            _orbs.Add(orb2);

            GameObject orb3Go = Instantiate(orbPrefab, spawnSpace);
            orb3Go.SetActive(true);
            var orb3 = orb3Go.GetComponent<OrbBehaviour>();
            orb3.SetOrbital(240, player.transform);
            _orbs.Add(orb3);
        }

        public void OnClickRelease()
        {
            if (_orbs.Count>0)
            {
                _orbs[0].SetRelease(testTarget_standard.transform);
                _orbs.RemoveAt(0);
            }
        }
    }
}