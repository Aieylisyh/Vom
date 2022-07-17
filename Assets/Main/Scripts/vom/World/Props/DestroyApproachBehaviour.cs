using UnityEngine;
using com;
using System.Collections.Generic;

namespace vom
{
    public class DestroyApproachBehaviour : MonoBehaviour
    {
        public List<ItemData> rewards;
        public GameObject vfx;
        public GameObject targetItem;

        private void Awake()
        {
            if (targetItem == null)
                targetItem = gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
                go.SetActive(true);

                SoundService.instance.Play("rockDestory");
                //CameraShake.instance.Shake(CameraShake.ShakeLevel.Weak);
                SpawnLoot();
                Destroy(targetItem);
            }
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