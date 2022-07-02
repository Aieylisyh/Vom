﻿using UnityEngine;
using com;

namespace vom
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public Animator animator;
        public CharacterController cc;
        public Transform circleTrans;

        public EnemyHealthBehaviour health { get; private set; }
        public EnemyAttackBehaviour attack { get; private set; }
        public EnemyMoveBehaviour move { get; private set; }
        public EnemyTargetSearcherBehaviour targetSearcher { get; private set; }
        public EnemyDeathBehaviour death { get; private set; }

        private void Awake()
        {
            health = GetComponent<EnemyHealthBehaviour>();
            attack = GetComponent<EnemyAttackBehaviour>();
            move = GetComponent<EnemyMoveBehaviour>();
            targetSearcher = GetComponent<EnemyTargetSearcherBehaviour>();
            death = GetComponent<EnemyDeathBehaviour>();
        }

        public void Start()
        {
            EnemySystem.instance.AddEnemy(this);
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

        public void OnHit(OrbBehaviour orb)
        {
            if (death.dead)
                return;

            health.ReceiveDamage(orb.dmg);
            targetSearcher.OnAttacked();
            animator.SetTrigger("Wound");
        }
    }
}