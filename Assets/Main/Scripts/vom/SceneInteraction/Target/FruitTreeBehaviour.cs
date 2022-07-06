using UnityEngine;
using com;
using DG.Tweening;

namespace vom
{

    public class FruitTreeBehaviour : SceneInteractionTargetBehaviour
    {
        public Transform tree;

        public GameObject fruits;

        private void Start()
        {
            if (targetItem == null)
            {
                targetItem = gameObject;
            }
        }

        public void Chopped()
        {
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);
            SoundService.instance.Play("wood hit");

            tree.DOShakeRotation(0.5f, new Vector3(30, 0, 30), 8);
        }

        public void FinishChop()
        {
            Chopped();
            SoundService.instance.Play("rockDestory");
            CameraShake.instance.Shake(CameraShake.ShakeLevel.Weak);
            for (int i = 0; i < 2; i++)
            {
                LootSystem.instance.SpawnGold(transform.position, new ItemData((int)data.baseAmount, "Gold"), i);
            }

            Destroy(targetItem);
        }
    }
}