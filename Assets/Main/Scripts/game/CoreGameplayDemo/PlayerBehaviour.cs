using UnityEngine;
using System.Collections;

namespace vom
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static PlayerBehaviour instance;

        public Animator animator;

        public CharacterController cc;

        private Vector2 _moveDist;

        public float speed;

        public float rotationLerpFactor = 0.1f;
        private Vector3 _lastEular;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            _lastEular = transform.forward;
        }

        void Update()
        {
            Move();

            Rotate();
        }

        void Rotate()
        {
            var dir = Vector3.Lerp(transform.forward, _lastEular, rotationLerpFactor);
            transform.rotation = Quaternion.LookRotation(dir);
        }

        public void ReceiveMoveInput(Vector2 dir)
        {
            _moveDist = dir.normalized;
        }

        void Move()
        {
            if (_moveDist.magnitude == 0)
            {
                animator.SetBool("move", false);
            }
            else
            {
                animator.SetBool("move", true);
                var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
                cc.SimpleMove(deltaDist * speed);
                _lastEular = deltaDist;
            }

            _moveDist = Vector2.zero;
        }
    }
}