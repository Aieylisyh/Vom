using UnityEngine;
using com;
using DG.Tweening;
using System.Collections.Generic;

namespace vom
{

    public class FruitTreeBehaviour : SceneInteractionTargetBehaviour
    {
        public Transform tree;

        public GameObject fruits;

        List<GameObject> _attachedFruits;
        public GameObject fruitPrefab;

        public bool hasFruit { get; private set; }


        public Transform[] fruitTransList;

        private void Start()
        {
            if (targetItem == null)
                targetItem = gameObject;

            InitFruits();
            tree.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }

        void InitFruits()
        {
            if (interaction == ESceneInteraction.Tree && fruits != null && Random.value < ConfigSystem.instance.sceneInteractionConfig.treeHasFruitChance)
            {
                interaction = ESceneInteraction.Fruit;
                hasFruit = true;
                fruits.SetActive(true);
            }

            return;

            ClearAttachedFruits();
            bool hasAttachedFruit = Random.value < ConfigSystem.instance.sceneInteractionConfig.treeHasFruitChance;
            if (hasAttachedFruit)
            {
                int count = Random.Range(1, 4);
                if (count > 3)
                    count = 3;

                for (var i = 0; i < count; i++)
                {
                    var go = Instantiate(fruitPrefab, fruitTransList[i].position, fruitTransList[i].rotation, transform);
                    // go.SetActive(true);
                    _attachedFruits.Add(go);
                }
            }
        }

        void ClearAttachedFruits()
        {
            if (_attachedFruits != null)
            {
                foreach (var f in _attachedFruits)
                {
                    if (f != null)
                        Destroy(f);
                }
            }
            _attachedFruits = new List<GameObject>();
        }

        void SpawnLootableFruits()
        {
            var count = _attachedFruits.Count;
            foreach (var f in _attachedFruits)
            {
                LootSystem.instance.SpawnLoot(f.transform.position, new ItemData(_attachedFruits.IndexOf(f) == 0 ? count : 0, "Apple"));
            }

            ClearAttachedFruits();
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
                LootSystem.instance.SpawnLoot(transform.position, new ItemData(Random.Range(1, 1000), "Gold"));
            }
            LootSystem.instance.SpawnLoot(transform.position, new ItemData(1, "Wood"));
            Destroy(targetItem);
        }

        public void FinishFruit()
        {
            var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            go.SetActive(true);

            SoundService.instance.Play(new string[3] { "vega1", "vega2", "vega3" });

            for (int i = 0; i < 2; i++)
            {
                LootSystem.instance.SpawnLoot(transform.position, new ItemData(3, "Apple"));
            }
            // SpawnLootableFruits();
            hasFruit = false;
            fruits.SetActive(false);
            interaction = ESceneInteraction.Tree;

            triggered = false;
        }
    }
}