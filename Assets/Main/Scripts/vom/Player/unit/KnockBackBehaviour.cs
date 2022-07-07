using UnityEngine;

namespace vom
{
    public class KnockBackBehaviour : MonoBehaviour
    {
        CharacterController _cc;
        public float dec;
        float _speed;
        Vector3 _dir;

        public void setCc(CharacterController cc)
        {
            _cc = cc;
        }

        public void KnockBack(Vector3 dir, float speed)
        {
            _dir = dir;
            _speed = speed;
        }

        void Update()
        {
            if (_speed <= 0)
                return;

            _cc.Move(_dir * _speed);
            _speed -= dec * com.GameTime.deltaTime;
        }
    }
}