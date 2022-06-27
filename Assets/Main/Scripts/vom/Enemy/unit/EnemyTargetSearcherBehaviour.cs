using UnityEngine;
using com;

namespace vom
{
    public class EnemyTargetSearcherBehaviour : VomEnemyComponent
    {
        public bool alerted { get; private set; }
        public Transform target { get; private set; }
        float _alertTimer;

        public AttackRange sightRange = AttackRange.Sight;
        float _fSightRan;
        bool _hasTriggered;
        Vector3 _toPlayerDir;

        public float targetDist { get; private set; }

        protected override void Start()
        {
            _fSightRan = CombatSystem.GetRange(sightRange);
            _alertTimer = 0;
            target = null;
            alerted = false;
            _hasTriggered = false;
            base.Start();
        }

        public void RepositionDone()
        {
            alerted = false;
        }

        void CheckSight()
        {
            var player = PlayerBehaviour.instance;
            if (player.health.dead)
            {
                ExitAlert();
                return;
            }

            var playerPos = player.transform.position;
            _toPlayerDir = playerPos - transform.position;
            targetDist = (_toPlayerDir).magnitude;
            if (targetDist < _fSightRan)
            {
                target = player.transform;
                EnterAlert();
                return;
            }
        }

        public void OnUpdate()
        {
            CheckSight();

            if (alerted)
            {
                if (_alertTimer > 0)
                {
                    _alertTimer -= GameTime.deltaTime;
                    if (_alertTimer <= 0)
                    {
                        alerted = false;
                    }
                }
            }
        }

        public void OnAttacked()
        {
            target = PlayerBehaviour.instance.transform;
            EnterAlert();
        }

        void EnterAlert()
        {
            _alertTimer = CombatSystem.enemyAlertTime;
            alerted = true;
        }

        public void ExitAlert()
        {
            alerted = false;
            target = null;
            _hasTriggered = true;
        }

        public bool IsInPlayerView()
        {
            var dX = _toPlayerDir.x;
            var dZ = _toPlayerDir.z;
            //Debug.Log(dX + " " + dZ + " | " + MapSystem.instance.tileNumRight + " " + MapSystem.instance.tileNumForward + " " + MapSystem.instance.tileNumBackward);
            if (Mathf.Abs(dX) < MapSystem.instance.tileNumRight)
            {
                if (-dZ < MapSystem.instance.tileNumForward && dZ < MapSystem.instance.tileNumBackward)
                {
                    return true;
                }
            }

            return false;
        }
    }
}