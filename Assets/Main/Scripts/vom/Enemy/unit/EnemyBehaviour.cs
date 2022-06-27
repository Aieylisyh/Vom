using UnityEngine;
using com;

namespace vom
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public Animator animator;
        public Transform circleTrans;
        public EnemyHealthBehaviour health;
        public EnemyAttackBehaviour attack;

        public void Start()
        {
            health.Init();
        }

        private void Update()
        {
            if (!health.dead)
            {
                attack.Attack();
            }
        }

        public void OnHit(OrbBehaviour orb)
        {
            // CameraShake.instance.Shake(orb.hitShakeLevel);
            health.ReceiveDamage(orb.dmg);
            animator.SetTrigger("Wound");
        }
    }
}