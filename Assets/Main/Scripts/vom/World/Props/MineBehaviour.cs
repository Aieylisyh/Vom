using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using com;

namespace vom
{
    public class MineBehaviour : SceneInteractionTargetBehaviour
    {
        public List<ItemData> rewards;

        public ParticleSystem ps;

        private void Start()
        {
            if (ConfigSystem.instance == null)
                return;

            if (Random.value > ConfigSystem.instance.sceneInteractionConfig.mineBaseChance)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void SliceFeedback()
        {
            ps.Play();
            SoundService.instance.Play("rockDestory");
        }

        public void FinishMining()
        {
            SoundService.instance.Play("stone");
            CameraShake.instance.Shake(CameraShake.ShakeLevel.VeryWeak);
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);
            //  var amplitude = 30f;
            //  tree.DOShakeRotation(0.5f, new Vector3(amplitude, 0, amplitude), 8);

            SpawnLoot();
            Destroy(gameObject);
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