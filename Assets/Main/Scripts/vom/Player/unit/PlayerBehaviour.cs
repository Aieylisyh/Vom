using UnityEngine;

namespace vom
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static PlayerBehaviour instance;
        public Animator animator;
        public CharacterController cc;
        public PlayerAttackBehaviour attack;
        public PlayerMoveBehaviour move;

        void Awake()
        {
            instance = this;
        }

        void Update()
        {
            if (isAlive)
            {
                attack.Attack();
                move.Move();
            }
        }

        public bool isAlive { get { return true; } }
    }
}