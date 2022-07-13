using UnityEngine;
using com;
using System.Collections.Generic;

namespace vom
{
    public class CombatSystem : MonoBehaviour
    {
        public static CombatSystem instance { get; private set; }

        public Transform projectileSpace;

        public float r_Melee = 1.75f;
        public float r_MeleeBig = 2.5f;
        public float r_Short = 6.5f;
        public float r_EneSight = 6.75f;
        public float r_Mid = 7.75f;
        public float r_MidLong = 9.25f;
        public float r_Long = 9.75f;
        public float r_Sight = 10.25f;
        public float r_VeryLong = 15.0f;

        public HpBarBehaviour hpBarPrefab;
        public AlertBehaviour alertPrefab;

        void Awake()
        {
            instance = this;
        }

        public static float GetRange(AttackRange range)
        {
            switch (range)
            {
                case AttackRange.Melee:
                    return instance.r_Melee;

                case AttackRange.MeleeBig:
                    return instance.r_MeleeBig;

                case AttackRange.Short:
                    return instance.r_Short;

                case AttackRange.EneSight:
                    return instance.r_EneSight;

                case AttackRange.Mid:
                    return instance.r_Mid;

                case AttackRange.MidLong:
                    return instance.r_MidLong;

                case AttackRange.Long:
                    return instance.r_Long;

                case AttackRange.Sight:
                    return instance.r_Sight;

                case AttackRange.VeryLong:
                    return instance.r_VeryLong;
            }

            return 0;
        }
    }

    public enum AttackRange
    {
        None = 0,
        Melee = 10,
        MeleeBig = 20,
        Short = 30,
        EneSight = 40,
        Mid = 50,
        MidLong = 60,
        Long = 70,
        Sight = 80,
        VeryLong = 100,
    }
}