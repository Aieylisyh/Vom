using UnityEngine;
using com;
namespace game
{
    public class BombAi : UnitAi
    {
        public float rotateMax = 100;
        public float rotateMin = 50;
        public RotateBehaviour rotateBehaviour;

        public override void ResetState()
        {
            rotateBehaviour.SpeedY = 0;
            rotateBehaviour.SpeedZ = Random.Range(rotateMin, rotateMax);
            rotateBehaviour.SpeedX = 0;
            if (Random.value < 0.5f)
            {
                rotateBehaviour.SpeedZ = -rotateBehaviour.SpeedZ;
            }
            var e = rotateBehaviour.transform.localEulerAngles;
            e.x = 0;
            e.y = Random.Range(-80, 80);
            e.z = Random.Range(-45, 45);
            rotateBehaviour.transform.localEulerAngles = e;
        }

        protected override void Tick()
        {
            self.move.Move();
        }
    }
}
