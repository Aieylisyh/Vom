using UnityEngine;
using com;

namespace game
{
    public class BombMove : UnitMove
    {
        private bool _isShard;

        public float shardUpwardStartSpeed = -2f;
        public float gravity = 10;
        public float speedExit = 3.3f;
        private float _shardUpwardSpeed;
        private float _shardRightSpeed;
        public float shardRightDec = 1;
        public TrailRenderer tr;

        public void SetShard(bool enabled, float speedX = 0)
        {
            tr.enabled = enabled;
            _isShard = enabled;
            _shardUpwardSpeed = shardUpwardStartSpeed;
            _shardRightSpeed = speedX;
        }

        public override void Move()
        {
            if (_isShard)
            {
                ShardMove();
            }
            else
            {
                Translate(dir);
            }
        }

        void ExitShardState()
        {
            tr.enabled = false;
            _isShard = false;
            (self as Bomb).ExitShardState();
        }

        void ShardMove()
        {
            if (_shardUpwardSpeed < speedExit)
            {
                _shardUpwardSpeed += gravity * GameTime.deltaTime;
            }
            else
            {
                ExitShardState();
                return;
            }

            if (Mathf.Approximately(_shardRightSpeed, 0))
            {
                _shardRightSpeed = 0;
            }
            else
            {
                _shardRightSpeed += (_shardRightSpeed < 0 ? shardRightDec : -shardRightDec) * GameTime.deltaTime;
            }

            var dirMerged = new Vector3(_shardRightSpeed, -_shardUpwardSpeed, 0);
            rotateAlignMove?.Rotate(dirMerged);
            transform.position += dirMerged * GameTime.deltaTime;
        }
    }
}