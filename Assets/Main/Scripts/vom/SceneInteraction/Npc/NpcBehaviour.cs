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

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (CanInteract())
                {
                    MmoCameraCinematicSystem.instance.NpcView(other.transform, this);
                }
            }
        }

        bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            animator.SetTrigger(PlayerAnimeParams.jump);
            ps.Play();
        }
    }
}