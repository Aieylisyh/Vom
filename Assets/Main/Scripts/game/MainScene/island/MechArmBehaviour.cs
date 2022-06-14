using UnityEngine;
using com;

namespace game
{
    public class MechArmBehaviour : MonoBehaviour
    {

        //public Transform hinge1;
        //public Transform hinge2;
        public Transform hinge3;
        //public float hinge1Speed = 0;
        //public float hinge2Speed = 0;
        public float hinge3Speed = 0;

        public Animator animator;
        public bool hasDelay;

        private void OnEnable()
        {
            RestartArms();
        }

        public void RestartArms()
        {
            animator.SetTrigger("restart");
            if (hasDelay)
            {
                animator.SetTrigger("mech1");
            }
            else
            {
                animator.SetTrigger("mech2");
            }
        }

        void Update()
        {
            // hinge1.Rotate(Vector3.right, Time.deltaTime * hinge1Speed, Space.Self);
            if (hinge3Speed != 0)
            {
                hinge3.Rotate(Vector3.forward, Time.deltaTime * hinge3Speed, Space.Self);
            }
        }
    }
}