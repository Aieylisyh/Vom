using UnityEngine;
using com;
using System.Collections.Generic;

namespace vom
{
    public class CombatSystem : MonoBehaviour
    {
        public static CombatSystem instance { get; private set; }

        public Transform EnemyProjectileSpace;

        void Awake()
        {
            instance = this;
        }

        public float GetRange(AttackRange range)
        {
            switch (range)
            {
                case AttackRange.None:
                    return 0;

                case AttackRange.Melee:
                    return 1f;

                case AttackRange.MeleeBig:
                    return 1.5f;

                case AttackRange.Short:
                    return 4f;

                case AttackRange.Mid:
                    return 7f;

                case AttackRange.Long:
                    return 10f;

                case AttackRange.VeryLong:
                    return 15f;
            }

            return 0;
        }
    }

    public enum AttackRange
    {
        None,
        Melee,
        MeleeBig,
        Short,
        Mid,
        Long,
        VeryLong,
    }
}