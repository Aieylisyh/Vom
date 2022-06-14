using UnityEngine;
using com;
namespace game
{
    public class LaserLauncherMove : EnemyMove
    {
        public float llShowUpDec = 2;
        public float llShowUpTime = 2;
        public override void ResetState()
        {
            base.ResetState();
            _floatOffsetTime = 0;

            _showUpTime = llShowUpTime;
            _showUpTimer = llShowUpTime;
            _showUpSpeed = llShowUpDec * llShowUpTime;
        }

        protected override void BasicMove()
        {
            var pos = transform.position;
            pos.z = pos.z * 0.96f;
            transform.position = pos;
            shipFloat();
            //Translate(dir);
        }

        protected override void ShowUp()
        {
            _showUpTimer -= com.GameTime.deltaTime;
            if (_showUpTimer < 0)
            {
                _showUpTimer = 0;
                _showUpSpeed = 0;
            }
            else
            {
                _showUpSpeed -= com.GameTime.deltaTime * llShowUpDec;
            }

            Speed = _showUpSpeed;
            Translate(Vector3.up);
        }
    }
}
