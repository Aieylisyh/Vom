using UnityEngine;
using com;
using System.Collections.Generic;

namespace vom
{
    public class CombatSystem : MonoBehaviour
    {
        public static CombatSystem instance { get; private set; }

        public Transform projectileSpace;

        void Awake()
        {
            instance = this;
        }

        public static float GetRange(AttackRange range)
        {
            switch (range)
            {
                case AttackRange.None:
                    return 0;

                case AttackRange.Melee:
                    return 1.75f;

                case AttackRange.MeleeBig:
                    return 2.25f;

                case AttackRange.Short:
                    return 6.5f;

                case AttackRange.Mid:
                    return 7.75f;

                case AttackRange.MidLong:
                    return 9.25f;

                case AttackRange.Long:
                    return 9.75f;

                case AttackRange.Sight:
                    return 10.25f;

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
        MidLong,
        Long,
        Sight,
        VeryLong,
    }
}