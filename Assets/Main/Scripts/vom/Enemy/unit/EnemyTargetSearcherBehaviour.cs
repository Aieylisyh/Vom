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

        static float _ticktime;

        public override void ResetState()
        {
            _ticktime = ConfigSystem.instance.enemyConfig.alertTickTime;
            _fSight = CombatSystem.GetRange(host.proto.sightRange);
            _alertTimestamp = GameTime.time + _ticktime;
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
                // Debug.Log("alerted " + _alertTurns);
                _alertTurns -= 1;
                if (_alertTurns == ConfigSystem.instance.enemyConfig.alertSpreadTurn)
                    SpreadAlert();

                if (alertOrigin != null)
                    targetDist = (alertOrigin.position - transform.position).magnitude;

                if (_alertTurns <= 0 || alertOrigin == null)
                {
                    _alertTurns = 0;
                    alerted = false;
                }
            }
        }

        void SpreadAlert()
        {
            var dist = ConfigSystem.instance.enemyConfig.alertSpreadRange;

            foreach (var e in EnemySystem.instance.enemies)
            {
                if (!e.death.dead && Vector3.Distance(e.transform.position, transform.position) < dist)
                {
                    e.targetSearcher.TryEnterAlert(this.alertOrigin);
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
                    TryEnterAlert(p.transform);
                    break;
                }
            }
        }

        public bool InSight(Vector3 targetPos, Vector3 selfPos)
        {
            var _toDir = targetPos - transform.position;
            var dist = (_toDir).magnitude;

            if (dist < CombatSystem.GetRange(AttackRange.Melee))
                return true;
            if (dist < _fSight && Vector3.Angle(_toDir, host.move.rotatePart.forward) < EnemyService.GetCfg().angleSight)
                return true;

            return false;
        }

        public void OnUpdate()
        {
            if (GameTime.time > _alertTimestamp)
            {
                _alertTimestamp += _ticktime;
                CheckAlert();
                CheckSight();
            }
        }

        public void OnAttacked(Transform origin)
        {
            if (origin != null)
                TryEnterAlert(origin);
        }

        void TryEnterAlert(Transform origin)
        {
            if (host.move.isRunningBack)
            {
                ExitAlert();
                return;
            }

            alertOrigin = origin;
            targetDist = (origin.position - transform.position).magnitude;
            _alertTurns = ConfigSystem.instance.enemyConfig.alertTurns;

            if (alerted)
            {
                return;
            }

            alerted = true;
            if (alertView != null)
                Debug.LogWarning("alert is continued after duration!");
            else
                alertView = EnemySystem.instance.CreateAlertView(transform, (host.sizeValue) * 520 - 20, host.sizeValue * 0.9f + 0.3f);
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