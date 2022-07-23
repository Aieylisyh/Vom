using UnityEngine;
using com;

namespace vom
{
    public class PlayerMoveBehaviour : VomPlayerComponent
    {
        private bool _draging;
        private Vector2 _startPos;
        private Vector2 _lastPos;
        public float ignoreThreshold = 10;

        private Vector2 _moveDist;
        private bool _lastUpdateMoved;

        public float speed;
        public Transform rotatePart;

        public TouchViewBehaviour touchView;

        public float camDistSpeed;
        public float movingCamDist;
        public float stayCamDist;

        public MmoCameraBehaviour mmoCamera;
        public KnockBackBehaviour knockBack { get; private set; }
        public DragIndicatorBehaviour dragIndicator;

        public override void ResetState()
        {
            knockBack = GetComponent<KnockBackBehaviour>();
            knockBack.setCc(host.cc);
            _lastUpdateMoved = false;
        }

        public void StartDrag(Vector2 pos)
        {
            _draging = true;
            _startPos = pos;
            _lastPos = pos;
            host.combat.ShowHud(false);
        }

        public void UpdateDrag(Vector2 pos)
        {
            _lastPos = pos;
        }

        public void EndDrag()
        {
            _draging = false;
            touchView.Hide();
            dragIndicator.Hide();
            host.combat.UpdateState();
        }

        public void Moved()
        {
            SoundService.instance.Play(new string[2] { "step1", "step2" });
        }

        protected override void Update()
        {
            base.Update();

            if (_draging)
            {
                var delta = _lastPos - _startPos;
                if (delta.magnitude > ignoreThreshold)
                {
                    _moveDist = delta.normalized;
                    dragIndicator.Hide();
                }
                else
                {
                    dragIndicator.Show();
                }

                touchView.Show(delta);

                if (mmoCamera.parameters.distance < movingCamDist)
                    mmoCamera.parameters.distance += camDistSpeed * Time.deltaTime;
            }
            else
            {
                if (!host.combat.isInCombat && mmoCamera.parameters.distance > stayCamDist)
                    mmoCamera.parameters.distance -= camDistSpeed * Time.deltaTime;
            }
        }

        public bool isMoving { get { return _moveDist.magnitude > 0f; } }

        public void Move()
        {
            if (_moveDist.magnitude == 0)
            {
                if (_lastUpdateMoved)
                {
                    if (host.animator.GetBool(PlayerAnimeParams.move))
                        host.animator.SetBool(PlayerAnimeParams.move, false);
                    host.combat.UpdateState();
                    _lastUpdateMoved = false;
                }

                Fall();
                _lastUpdateMoved = false;
            }
            else
            {
                if (!_lastUpdateMoved)
                {
                    host.interaction.HideAll();
                    host.combat.ShowHud(false);
                    _lastUpdateMoved = true;
                }

                if (!host.animator.GetBool(PlayerAnimeParams.move))
                    host.animator.SetBool(PlayerAnimeParams.move, true);
                var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
                //host.cc.SimpleMove(deltaDist * speed);
                host.cc.Move((deltaDist * speed - 8f * Vector3.up) * GameTime.deltaTime);
                Rotate(deltaDist);
            }

            _moveDist = Vector2.zero;
        }

        //void OnControllerColliderHit(ControllerColliderHit hit)
        //{
        //    Debug.Log("OnControllerColliderHit");
        //    Debug.Log(hit.gameObject);
        //}

        void Fall()
        {
            if (!host.cc.isGrounded)
                host.cc.Move(-8f * Vector3.up * GameTime.deltaTime);
        }

        public void Rotate(Vector3 to)
        {
            to.y = 0;
            if (to.magnitude <= 0)
                return;

            rotatePart.rotation = Quaternion.LookRotation(to);
        }
    }
}