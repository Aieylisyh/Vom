using UnityEngine;
using com;
using DG.Tweening;

namespace vom
{

    public class FruitTreeBehaviour : SceneInteractionTargetBehaviour
    {
        public Transform tree;

        public GameObject fruits;

        public bool hasFruit { get; private set; }

        private void Start()
        {
            if (targetItem == null)
                targetItem = gameObject;

            if (interaction == ESceneInteraction.Tree && fruits != null && Random.value < game.ConfigService.instance.sceneInteractionConfig.treeHasFruitChance)
            {
                interaction = ESceneInteraction.Fruit;
                hasFruit = true;
                fruits.SetActive(true);
            }

            tree.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }

        public void Chopped(bool weak = true)
        {
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);
            SoundService.instance.Play("wood hit");
            var amplitude = 30f;
            if (weak)
                amplitude = 18;
            tree.DOShakeRotation(0.5f, new Vector3(amplitude, 0, amplitude), 8);
        }

        public void FinishChop()
        {
            Chopped();
            SoundService.instance.Play("rockDestory");
            CameraShake.instance.Shake(CameraShake.ShakeLevel.Weak);
            for (int i = 0; i < 2; i++)
            {
                LootSystem.instance.SpawnGold(transform.position, new ItemData((int)data.baseAmount, "Gold"));
            }

            Destroy(targetItem);
        }

        public void FinishFruit()
        {
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);

            SoundService.instance.Play("fruit");

            for (int i = 0; i < 2; i++)
            {
                LootSystem.instance.SpawnGold(transform.position, new ItemData((int)data.baseAmount, "Gold"));
            }

            hasFruit = false;
            fruits.SetActive(false);
            interaction = ESceneInteraction.Tree;
        }
    }
}