using UnityEngine;

namespace vom
{
    public class RuntimeSpawnBehaviour : MonoBehaviour
    {
        public GameObject prefab;

        protected virtual void Start()
        {
            Spawn();
        }

        protected virtual void Spawn()
        {
            Instantiate(prefab, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        }
    }
}