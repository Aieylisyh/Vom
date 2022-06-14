using UnityEngine;

namespace game
{
    public class UnitMove : UnitComponent
    {
        public RotateAlignMove rotateAlignMove;
        public float Speed = 3;
        public Vector3 dir { get; protected set; }

        public void SetDir(Vector3 dir)
        {
            this.dir = dir;
        }

        public override void ResetState()
        {
            base.ResetState();
        }

        //to note, d is normalized!
        protected virtual void Translate(Vector3 d)
        {
            AlignDir(d);
            transform.position += d * Speed * com.GameTime.deltaTime;
        }

        protected virtual void AlignDir(Vector3 d)
        {
            rotateAlignMove?.Rotate(d);
        }

        public virtual void Move()
        {
            Translate(dir);
        }

        public virtual void MoveLeft()
        {
            Translate(Vector3.left);
        }

        public virtual void MoveRight()
        {
            Translate(Vector3.right);
        }
    }
}
