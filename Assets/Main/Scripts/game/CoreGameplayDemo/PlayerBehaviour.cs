using UnityEngine;
using System.Collections;

namespace vom
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public static PlayerBehaviour instance;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {

        }

        void Update()
        {
            Move();
        }

        public void ReceiveMoveInput(Vector2 dir)
        {

        }

        void Move()
        {

        }
    }
}