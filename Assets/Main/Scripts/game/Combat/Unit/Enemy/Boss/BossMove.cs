using System;
using com;
using DG.Tweening;
using UnityEngine;

namespace game
{
    public class BossMove : EnemyMove
    {
        private float _turnSpeedCache;

        public override void TurnBack(float duration)
        {
            _disableAlignDir = true;
            _turnSpeedCache = Speed;
            Speed = 0;

            Vector3 v = new Vector3(0, goingRight ? 180 : 0, 0);
            rotateAlignMove.trans.DOLocalRotate(v, duration).OnComplete(() =>
            {
                dir = goingRight ? Vector3.right : Vector3.left;
                _disableAlignDir = false;
                Speed = _turnSpeedCache;
            });

            goingRight = !goingRight;
        }
    }
}
