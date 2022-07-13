using UnityEngine;
using com;

namespace vom
{
    public class EnemyTargetSearcherBehaviour : VomEnemyComponent
    {
        public bool alerted { get; private set; }
        public Vector3 alertOrigin { get; private set; }
        public Transform alertTransform { get; private set; }

        float _alertTimer;

        float _fSight;

        Vector3 _toPlayerDir;

        public float targetDist { get; private set; }

        bool _isInPlayerView;
        int _checkedFrame;

        public override void ResetState()
        {
            _fSight = CombatSystem.GetRange(host.proto.sightRange);
            _alertTimer = 0;
            ExitAlert();
        }

        public void RepositionDone()
        {
            ExitAlert();
        }

        void CheckSight()
        {
            var players = EnemySystem.instance.GetValidPlayers();
            if (players == null)
            {
                ExitAlert();
                return;
            }

            foreach (var p in players)
            {
                var targetPos = p.transform.position;
                var selfPos = transform.position;
                if (InSight(targetPos, selfPos))
                {
                    var _toDir = targetPos - transform.position;
                    targetDist = (_toDir).magnitude;
                    if (targetDist < _fSight)
                    {
                        alertOrigin = targetPos;

                        TryEnterAlert();
                        return;
                    }
                }
            }

        }

        public bool InSight(Vector3 targetPos, Vector3 selfPos)
        {
            return false;
        }

        public void OnUpdate()
        {
            if (alerted)
            {
                if (_alertTimer > 0)
                {
                    _alertTimer -= GameTime.deltaTime;
                    if (_alertTimer <= 0)
                        alerted = false;
                }
            }
            CheckSight();
        }

        public void OnAttacked(Vector3 origin)
        {
            alertOrigin = origin;
            TryEnterAlert();
        }

        void TryEnterAlert()
        {
            if (alerted)
                return;

            if (host.move.isRunningBack)
            {
                ExitAlert();
                return;
            }

            _alertTimer = ConfigSystem.instance.combatConfig.enemy.alertTime;
            alerted = true;
        }

        public void ExitAlert()
        {
            alerted = false;
            alertTransform = null;
        }

        public bool IsInPlayerView()
        {
            if (Time.frameCount == _checkedFrame)
            {
                return _isInPlayerView;
            }

            _checkedFrame = Time.frameCount;
            _isInPlayerView = false;

            var dX = _toPlayerDir.x;
            var dZ = _toPlayerDir.z;
            //Debug.Log(dX + " " + dZ + " | " + MapSystem.instance.tileNumRight + " " + MapSystem.instance.tileNumForward + " " + MapSystem.instance.tileNumBackward);
            if (Mathf.Abs(dX) < MapSystem.instance.tileNumRight)
            {
                if (-dZ < MapSystem.instance.tileNumForward && dZ < MapSystem.instance.tileNumBackward)
                {
                    _isInPlayerView = true;
                }
            }

            return _isInPlayerView;
        }
    }
}