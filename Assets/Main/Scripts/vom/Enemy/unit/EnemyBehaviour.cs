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
                targetSearcher.OnUpdate();
                attack.Attack();
                move.Move();
            }
        }

        public void OnHit(OrbBehaviour orb)
        {
            if (health.dead)
                return;

            health.ReceiveDamage(orb.dmg);
            targetSearcher.OnAttacked();
            animator.SetTrigger("Wound");
        }
    }
}