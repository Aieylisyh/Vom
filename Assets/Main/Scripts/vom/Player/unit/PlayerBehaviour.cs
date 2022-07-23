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
        public PlayerSkillBehaviour skill;

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
            skill.ResetState();

            if (EnemySystem.instance.players.IndexOf(this) < 0)
                EnemySystem.instance.players.Add(this);

            combat.UpdateState();
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
            CameraShake.instance.Shake(orb.hitShakeLevel);
            OnHit(orb.dmg);
        }

        public void OnHit(int dmg)
        {
            if (health.dead)
                return;

            health.ReceiveDamage(dmg);
            combat.UpdateState();
        }

        public void LitMovement()
        {
            animator.SetTrigger(PlayerAnimeParams.jump);
        }
    }
}