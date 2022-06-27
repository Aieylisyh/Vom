using UnityEngine;
using com;

namespace vom
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static PlayerBehaviour instance { get; private set; }
        public Animator animator;
        public CharacterController cc;
        public PlayerAttackBehaviour attack;
        public PlayerMoveBehaviour move;
        public PlayerHealthBehaviour health;
        public PlayerCombatStateBehaviour combat;

        void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            health.Init();
        }

        void Update()
        {
            if (!health.dead)
            {
                attack.Attack();
                move.Move();
            }
        }

        public void OnHit(OrbBehaviour orb)
        {
            CameraShake.instance.Shake(orb.hitShakeLevel);
            health.ReceiveDamage(orb.dmg);
        }

        public void LitMovement()
        {
            animator.SetTrigger("jump");
        }
    }
}