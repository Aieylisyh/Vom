using UnityEngine;
using com;

namespace vom
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public Animator animator;
        public CharacterController cc;

        public Transform circleTrans;
        public EnemyHealthBehaviour health;
        public EnemyAttackBehaviour attack;
        public EnemyMoveBehaviour move;
        public EnemyTargetSearcherBehaviour targetSearcher;

        public void Start()
        {
            health.Init();
            EnemySystem.instance.AddEnemy(this);
        }

        private void OnDestroy()
        {
            EnemySystem.instance.RemoveEnemy(this);
        }

        private void Update()
        {
            if (!health.dead)
            {
                attack.Attack();
                move.Move();
                targetSearcher.OnUpdate();
            }
        }

        public void OnHit(OrbBehaviour orb)
        {
            if (health.dead)
            {
                return;
            }
            // CameraShake.instance.Shake(orb.hitShakeLevel);
            health.ReceiveDamage(orb.dmg);
            animator.SetTrigger("Wound");
        }
    }
}