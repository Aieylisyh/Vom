using UnityEngine;
using com;

namespace vom
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public Animator animator;
        public CharacterController cc;
        public Transform circleTrans;
        public float size = 0.25f;

        public EnemyHealthBehaviour health { get; private set; }
        public EnemyAttackBehaviour attack { get; private set; }
        public EnemyMoveBehaviour move { get; private set; }
        public EnemyTargetSearcherBehaviour targetSearcher { get; private set; }
        public EnemyDeathBehaviour death { get; private set; }

        public EnemyPrototype proto { get; private set; }

        private void Awake()
        {
            health = GetComponent<EnemyHealthBehaviour>();
            attack = GetComponent<EnemyAttackBehaviour>();
            move = GetComponent<EnemyMoveBehaviour>();
            targetSearcher = GetComponent<EnemyTargetSearcherBehaviour>();
            death = GetComponent<EnemyDeathBehaviour>();
        }

        public void AssignAttibution(EnemyPrototype pProto)
        {
            proto = pProto;
        }

        public void Start()
        {
            EnemySystem.instance.AddEnemy(this);

            circleTrans.localScale = Vector3.one * size;
            attack.ResetState();
            move.ResetState();
            health.ResetState();
            targetSearcher.ResetState();
            death.ResetState();
        }

        private void OnDestroy()
        {
            EnemySystem.instance.RemoveEnemy(this);
        }

        private void Update()
        {
            if (!death.dead)
            {
                targetSearcher.OnUpdate();
                attack.Attack();
                move.Move();
            }
        }

        public void OnHit(int dmg)
        {
            if (death.dead)
                return;

            health.ReceiveDamage(dmg);
            targetSearcher.OnAttacked();
            animator.SetTrigger("Wound");
        }

        public void KnockBack(Vector3 orgin, float knockBackForce)
        {
            if (death.dead)
                return;

            var dir = transform.position - orgin;
            dir.y = 0;
            move.knockBack.KnockBack(dir.normalized, knockBackForce);
        }

        public void OnHit(OrbBehaviour orb)
        {
            OnHit(orb.dmg);
        }
    }
}