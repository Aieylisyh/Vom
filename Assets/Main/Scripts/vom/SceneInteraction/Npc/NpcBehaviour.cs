using com;
using UnityEngine;

namespace vom
{
    public class NpcBehaviour : MonoBehaviour
    {
        public ChatBubbleBehaviour chatBubble;
        public NpcPrototype npc;
        public Animator animator;
        public ParticleSystem ps;
        public Transform rotatePart;

        bool _interacted;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (CanInteract())
                {
                    _interacted = true;
                    MmoCameraCinematicSystem.instance.NpcView(other.transform, this);
                }
            }
        }

        bool CanInteract()
        {
            return !_interacted;
        }

        public void OnInteract()
        {
            animator.SetTrigger(PlayerAnimeParams.jump);
            ps.Play();
        }

        public void Rotate(Vector3 to)
        {
            to.y = 0;
            if (to.magnitude <= 0)
                return;

            //Debug.Log(to);
            rotatePart.rotation = Quaternion.LookRotation(to);
        }
    }
}