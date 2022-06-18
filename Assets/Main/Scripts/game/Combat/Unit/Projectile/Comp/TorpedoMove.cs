using UnityEngine;
using com;
namespace game
{
    public class TorpedoMove : UnitMove
    {
        public float angularSpeed = 10;
        public float acc = 10;
        public float speedMax = 4;
        private Unit _target;
        public float startSpeed = 0;
        private Vector3 _primeDir;
        public float nonRotateTime = 0.25f;
        private float _rotateTimer;
        public float stopRotateTime = 0;
        private bool _drop = false;

        public bool useWave;
        private float _waveStartTime;
        private float _waveAmplitude = 1.8f;
        private float _waveFreq = 7f;
        private float _waveOffset;
        private Vector3 _wavePerpendicularDir;
        public float dropDec = 2f;
        public float dropStartSpeed = 2f;
        private float _dropSpeed;
        public float lifeTime = 9;
        private float _lifeTimer;

        public SearchTargetType searchTargetType;
        public enum SearchTargetType
        {
            Enemy,
            Player,
            EnemyProjectile,
            EnemyAndGhost,
            PlayerBomb,
            PlayerProjectile,
        }

        protected override void Tick()
        {
            _lifeTimer -= com.GameTime.deltaTime;
            if (_lifeTimer < 0)
            {
                self.death.Die(true);
                return;
            }

            if (Speed < speedMax)
            {
                Speed += acc * com.GameTime.deltaTime;
            }
            else
            {
                Speed = speedMax;
            }

            _rotateTimer += com.GameTime.deltaTime;
            if (nonRotateTime > 0 && _rotateTimer < nonRotateTime)
            {
                Drop();
                ZFix();
            }
            else if (stopRotateTime > 0 && _rotateTimer > stopRotateTime)
            {
                ZFix();
            }
            else
            {
                Rotate();
            }

            dir = dir.normalized * Speed;
            Move();
        }

        public override void Move()
        {
            base.Move();
            if (useWave)
            {
                var dt = com.GameTime.time - _waveStartTime;
                var delta = _wavePerpendicularDir * Mathf.Sin(dt * _waveFreq + _waveOffset) * _waveAmplitude * com.GameTime.deltaTime;
                if (dt < 1)
                {
                    delta *= Mathf.Lerp(0, 1, dt);
                }
                transform.position += delta;
            }
        }

        private void Drop()
        {
            if (!_drop)
                return;

            _dropSpeed -= com.GameTime.deltaTime * dropDec;
            if (_dropSpeed < 0)
            {
                _dropSpeed = 0;
            }
            var delta = Vector3.down * com.GameTime.deltaTime * _dropSpeed;
            transform.position += delta;
        }

        private float GetCurrentAngularSpeed()
        {
            var f = Speed / speedMax;
            return angularSpeed * f;
        }

        private void Rotate()
        {
            var tpDir = dir;
            if (_target != null && _target.IsAlive())
            {
                var idealDir = _target.move.transform.position - transform.position;
                tpDir = Vector3.RotateTowards(dir, idealDir, GetCurrentAngularSpeed() * com.GameTime.deltaTime, 0);
                SetDir(tpDir);
            }
            else
            {
                ZFix();
            }
        }

        private void ZFix()
        {
            //z fix
            var pos = transform.position;
            pos.z = pos.z * 0.95f;
            transform.position = pos;
            //var tpDir = dir;
            // tpDir = Vector3.RotateTowards(tpDir, _primeDir, angularSpeed * 0.5f * Time.deltaTime, 0);
            // SetDir(tpDir);
        }

        public override void ResetState()
        {
            base.ResetState();
            Speed = startSpeed;
            _rotateTimer = 0;
            _lifeTimer = lifeTime;
            Torpedo tor = self as Torpedo;
            _drop = false;

            switch (tor.torType)
            {
                case Torpedo.TorType.Direct:
                    var transPlayer = CombatService.instance.playerShip.move.transform;

                    switch (tor.directAimType)
                    {
                        case Torpedo.DirectAimType.None:
                            break;

                        case Torpedo.DirectAimType.PlayerLimitAngularDelta:
                            var p0 = transPlayer.position - transform.position;
                            var tpDir = Vector3.RotateTowards(Vector3.up, p0, tor.directAimOffset * Mathf.Deg2Rad, 0);
                            SetDir(tpDir);
                            break;
                        case Torpedo.DirectAimType.PlayerWithFixedOffset:
                            var deltaX = transPlayer.position.x - transform.position.x;
                            var p1 = transPlayer.position + tor.directAimOffset * ((deltaX < 0) ? Vector3.right : Vector3.left);
                            SetDir(p1 - transform.position);
                            break;
                        case Torpedo.DirectAimType.PlayerWithRandomOffset:
                            var p2 = transPlayer.position + tor.directAimOffset * ((Random.value < 0.5f) ? Vector3.right : Vector3.left);
                            SetDir(p2 - transform.position);
                            break;
                    }
                    break;

                case Torpedo.TorType.Trace:
                    useWave = false;
                    SearchTarget();
                    break;
            }
        }

        public void SetDrop(float dropStartSpeed, float pNoRotateTime)
        {
            _drop = true;
            _dropSpeed = dropStartSpeed;
            nonRotateTime = pNoRotateTime;
        }

        public void SetPrimeDir(Vector3 pDir)
        {
            _primeDir = pDir;
            if (useWave)
            {
                _wavePerpendicularDir = Vector3.Cross(_primeDir, Vector3.back).normalized;
            }
        }

        public void SearchTarget()
        {
            //Debug.Log("SearchTargets");

            if (_target == null || !_target.IsAlive())
            {
                if (searchTargetType == SearchTargetType.Player)
                {
                    if (CombatService.instance.playerShip != null && CombatService.instance.playerShip.IsAlive())
                    {
                        _target = CombatService.instance.playerShip;
                    }
                }
                else
                {
                    var list = CombatService.instance.units;
                    float dist = -1;
                    float tpDist = -1;
                    foreach (var e in list)
                    {
                        if (e == null || !e.IsAlive())
                        {
                            continue;
                        }

                        if (searchTargetType == SearchTargetType.Enemy)
                        {
                            if (!(e is Enemy))
                            {
                                continue;
                            }
                        }
                        if (searchTargetType == SearchTargetType.EnemyProjectile)
                        {
                            if (!(e.collision != null && e.collision.selfFlag == UnitCollision.CollisionFlag.EnemyProjectile))
                            {
                                continue;
                            }
                        }
                        if (searchTargetType == SearchTargetType.EnemyAndGhost)
                        {
                            if (!(e is Enemy))
                            {
                                if (!(e.collision != null && e.collision.selfFlag == UnitCollision.CollisionFlag.EnemyProjectile && e.attack.dmg.type == DamageType.Ghost))
                                {
                                    continue;
                                }
                            }
                        }
                        if (searchTargetType == SearchTargetType.PlayerBomb)
                        {
                            if (!(e.collision != null && e.collision.selfFlag == UnitCollision.CollisionFlag.PlayerProjectile))
                            {
                                continue;
                            }
                            if (!(e is Bomb))
                            {
                                continue;
                            }
                        }
                        if (searchTargetType == SearchTargetType.PlayerProjectile)
                        {
                            if (!(e.collision != null && e.collision.selfFlag == UnitCollision.CollisionFlag.PlayerProjectile))
                            {
                                continue;
                            }
                        }

                        Vector3 tpPos = e.move.transform.position;

                        tpDist = (tpPos - transform.position).magnitude;
                        if (dist < 0 || dist > tpDist)
                        {
                            _target = e;
                            dist = tpDist;
                        }
                    }

                }

                //Debug.Log("target is " + _target);
            }
        }

        public void SetWave(float turnOffset)
        {
            if (!useWave)
            {
                return;
            }

            _waveStartTime = com.GameTime.time;
            _waveOffset = turnOffset * Mathf.PI * 2;
        }
    }
}
