using UnityEngine;
using com;

namespace vom
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static PlayerBehaviour instance;
        public Animator animator;
        public CharacterController cc;
        public PlayerAttackBehaviour attack;
        public PlayerMoveBehaviour move;
        public PlayerHealthBehaviour health;

        void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            health.HealToFull();
        }

        void Update()
        {
            if (health.isAlive)
            {
                attack.Attack();
                move.Move();
            }
        }

        public void OnHit(OrbBehaviour orb)
        {
            CameraShake.instance.Shake(orb.hitShakeLevel);
            health.ReceiveDamage(orb.dmg);
            //animator.SetTrigger("Wound");
        }
    }
}