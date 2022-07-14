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

        public ParticleSystem psOutWater;
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
                psOutWater.Play(true);
                SoundService.instance.Play("water small");
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
            foreach (var r in rewards)
            {
                LootSystem.instance.SpawnLoot(transform.position+Vector3.up, r);
            }
        }
    }
}