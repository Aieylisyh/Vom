using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using com;

namespace vom
{
    public class FishingSpotBehaviour : SceneInteractionTargetBehaviour
    {
        public List<ItemData> rewards;

        public Animator animator;
        public static string key = "jump";
        public ParticleSystem psSuc;

        public float intervalMin;
        public float intervalMax;

        float _intervalTimer;

        private void Start()
        {
            if (ConfigSystem.instance == null)
                return;

            SetInterval();
        }

        void SetInterval()
        {
            _intervalTimer = GameTime.time + Random.Range(intervalMin, intervalMax);
        }

        private void Update()
        {
            if (GameTime.time > _intervalTimer)
            {
                SetInterval();
                animator.SetTrigger(key);
            }
        }

        public void FinishFishing()
        {
            SoundService.instance.Play("water big");
            psSuc.Play(true);
            SpawnLoot();

            triggered = false;
        }

        void SpawnLoot()
        {
            if (Random.value > ConfigSystem.instance.sceneInteractionConfig.fishBaseChance)
            {
                ToastSystem.instance.Add("The fish has left...");
                return;
            }

            foreach (var r in rewards)
            {
                LootSystem.instance.SpawnLoot(transform.position + Vector3.up * 1.5f, r);
            }
        }
    }
}