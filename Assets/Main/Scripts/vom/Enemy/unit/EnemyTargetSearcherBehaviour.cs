using UnityEngine;
using com;

namespace vom
{
    public class EnemyTargetSearcherBehaviour : VomEnemyComponent
    {
        public bool alerted { get; private set; }
        public Transform alertOrigin { get; private set; }

        float _alertTimestamp;
        int _alertTurns;

        float _fSight;

        Vector3 _toPlayerDir;

        public float targetDist { get; private set; }

        bool _isInPlayerView;
        int _checkedFrame;

        public AlertBehaviour alertView { get; private set; }

        public override void ResetState()
        {
            _fSight = CombatSystem.GetRange(host.proto.sightRange);
            _alertTimestamp = GameTime.time;
            ExitAlert();
        }

        public void RepositionDone()
        {
            ExitAlert();
        }

        void CheckAlert()
        {
            if (alerted)
            {
                Debug.Log("alerted " + _alertTurns);
                _alertTurns -= 1;
                if (_alertTurns <= 0 || alertOrigin == null)
                {
                    _alertTurns = 0;
                    alerted = false;
                }
            }
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
                    alertOrigin = p.transform;
                    TryEnterAlert();
                    break;
                }
            }
        }

        public bool InSight(Vector3 targetPos, Vector3 selfPos)
        {
            var _toDir = targetPos - transform.position;
            targetDist = (_toDir).magnitude;

            if (targetDist < CombatSystem.GetRange(AttackRange.Melee))
                return true;
            if (targetDist < _fSight && Vector3.Angle(_toDir, transform.forward) < EnemyService.GetCfg().angleSight)
                return true;

            return false;
        }

        public void OnUpdate()
        {
            if (GameTime.deltaTime > _alertTimestamp)
            {
                _alertTimestamp += ConfigSystem.instance.enemyConfig.alertTickTime;
                CheckAlert();
                CheckSight();
            }
        }

        public void OnAttacked(Transform origin)
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

            _alertTurns = ConfigSystem.instance.enemyConfig.alertTurns;
            alerted = true;

            if (alertView != null)
                Debug.LogWarning("alert is continued after duration!");
            else
                alertView = EnemySystem.instance.CreateAlertView(transform, (host.sizeValue - 0.1f) * 250, host.sizeValue * 1.3f + 0.38f);
        }

        public void ExitAlert()
        {
            alerted = false;
            alertOrigin = null;
            RemoveAlertView();
        }

        void RemoveAlertView()
        {
            if (alertView != null)
            {
                alertView.Hide();
                alertView = null;
            }
        }

        public bool IsInPlayerView()
        {
            if (Time.frameCount == _checkedFrame)
                return _isInPlayerView;

            _checkedFrame = Time.frameCount;
            _isInPlayerView = false;

            var dX = _toPlayerDir.x;
            var dZ = _toPlayerDir.z;
            //Debug.Log(dX + " " + dZ + " | " + MapSystem.instance.tileNumRight + " " + MapSystem.instance.tileNumForward + " " + MapSystem.instance.tileNumBackward);
            if (Mathf.Abs(dX) < MapSystem.instance.tileNumRight)
            {
                if (-dZ < MapSystem.instance.tileNumForward && dZ < MapSystem.instance.tileNumBackward)
                    _isInPlayerView = true;
            }

            return _isInPlayerView;
        }
    }
}