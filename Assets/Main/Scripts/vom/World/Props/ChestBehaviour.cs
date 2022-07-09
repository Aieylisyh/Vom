using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using com;

namespace vom
{
    public class ChestBehaviour : SceneInteractionTargetBehaviour
    {
        public GameObject chestLock;
        public GameObject cover;
        public Transform closedTrans;
        public Transform openTrans;
        public float openDuration;

        public List<ItemData> rewards;

        public bool opened { get; private set; }
        public bool locked { get; private set; }

        private void Start()
        {
            //debug
            Init(false, Random.value > 0.5f);
        }

        public void Init(bool pOpened, bool pLocked)
        {
            opened = pOpened;
            locked = pLocked;

            if (opened)
            {
                SetStateOpened();
            }
            else
            {
                SetStateClosed();
                chestLock.SetActive(locked);
            }
        }

        public void SetStateOpened()
        {
            chestLock.SetActive(false);
            cover.transform.SetPositionAndRotation(openTrans.position, openTrans.rotation);
        }

        public void SetStateClosed()
        {
            cover.transform.SetPositionAndRotation(closedTrans.position, closedTrans.rotation);
        }

        public override bool TestCanInteract()
        {
            base.TestCanInteract();
            bool hasKey = false;//TODO
            if (locked && !hasKey)
            {
                ToastSystem.instance.Add("I need a key to open it...");
                return false;
            }

            return true;
        }

        public void Open()
        {
            SoundService.instance.Play("rockDestory");
            CameraShake.instance.Shake(CameraShake.ShakeLevel.VeryWeak);
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);
            //  var amplitude = 30f;
            //  tree.DOShakeRotation(0.5f, new Vector3(amplitude, 0, amplitude), 8);

            cover.transform.DORotateQuaternion(openTrans.rotation, openDuration).SetEase(Ease.OutCubic);
            cover.transform.DOMove(openTrans.position, openDuration).SetEase(Ease.OutCubic);
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