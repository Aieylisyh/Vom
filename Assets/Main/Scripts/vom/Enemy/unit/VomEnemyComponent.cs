using UnityEngine;

namespace vom
{
    public class VomEnemyComponent : MonoBehaviour
    {
        protected EnemyBehaviour host;

        protected virtual void Awake()
        {
            host = GetComponent<EnemyBehaviour>();
            if (host == null)
            {
                host = GetComponentInParent<EnemyBehaviour>();
            }
        }

        protected virtual void Update()
        {
        }

        public virtual void ResetState()
        {

        }
    }
}