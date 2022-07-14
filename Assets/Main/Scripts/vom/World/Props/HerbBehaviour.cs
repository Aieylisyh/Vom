using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using com;

namespace vom
{
    public class HerbBehaviour : SceneInteractionTargetBehaviour
    {
        public List<ItemData> rewards;

        public bool opened { get; private set; }
        public bool locked { get; private set; }

        private void Start()
        {
            if (ConfigSystem.instance == null)
                return;
        }

        public void FinishHerbing()
        {
            SoundService.instance.Play("rockDestory");
            CameraShake.instance.Shake(CameraShake.ShakeLevel.VeryWeak);
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);
            //  var amplitude = 30f;
            //  tree.DOShakeRotation(0.5f, new Vector3(amplitude, 0, amplitude), 8);
            SpawnLoot();
        }

        void SpawnLoot()
        {
            foreach (var r in rewards)
            {
                LootSystem.instance.SpawnLoot(transform.position, r);
            }
        }
    }
}