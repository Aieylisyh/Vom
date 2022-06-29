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
        public PlayerInteractionCheckBehaviour interaction;

        void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            attack.ResetState();
            move.ResetState();
            health.ResetState();
            combat.ResetState();
            interaction.ResetState();
        }

        void Update()
        {
            attack.CheckMmoCameraOffset();

            if (!health.dead)
            {
                attack.Attack();
                move.Move();
            }
        }

        public void OnHit(OrbBehaviour orb)
        {
            if (health.dead)
                return;

            CameraShake.instance.Shake(orb.hitShakeLevel);
            health.ReceiveDamage(orb.dmg);
            combat.UpdateState();
        }

        public void LitMovement()
        {
            animator.SetTrigger("jump");
        }
    }
}