using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using com;

namespace vom
{
    public class DigHoleBehaviour : SceneInteractionTargetBehaviour
    {
        public List<ItemData> rewards;

        private void Start()
        {
            if (ConfigSystem.instance == null)
                return;

            if (Random.value > ConfigSystem.instance.sceneInteractionConfig.digHoleBaseChance)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void SliceFeedback()
        {
            //play ps
        }

        public void FinishDig()
        {
            SoundService.instance.Play("stone");
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);
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