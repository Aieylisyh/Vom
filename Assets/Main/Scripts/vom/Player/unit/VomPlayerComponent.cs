using UnityEngine;

namespace vom
{
    public class VomPlayerComponent : MonoBehaviour
    {
        protected PlayerBehaviour host;

        protected virtual void Awake()
        {
            host = GetComponent<PlayerBehaviour>();
            if (host == null)
            {
                host = GetComponentInParent<PlayerBehaviour>();
            }
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        public virtual void ResetState()
        {

        }
    }
}