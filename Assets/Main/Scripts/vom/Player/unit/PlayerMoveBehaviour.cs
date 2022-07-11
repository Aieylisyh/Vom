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

        public float speed;
        public Transform rotatePart;

        public TouchViewBehaviour touchView;

        public float camDistSpeed;
        public float movingCamDist;
        public float stayCamDist;

        public MmoCameraBehaviour mmoCamera;
        public KnockBackBehaviour knockBack { get; private set; }

        public override void ResetState()
        {
            knockBack = GetComponent<KnockBackBehaviour>();
            knockBack.setCc(host.cc);
        }

        public void StartDrag(Vector2 pos)
        {
            _draging = true;
            _startPos = pos;
            _lastPos = pos;
        }

        public void UpdateDrag(Vector2 pos)
        {
            _lastPos = pos;
        }

        public void EndDrag()
        {
            _draging = false;
            touchView.Hide();
        }

        public void Moved()
        {
            // com.SoundService.instance.Play("step");
        }

        protected override void Update()
        {
            base.Update();

            if (_draging)
            {
                var delta = _lastPos - _startPos;
                if (delta.magnitude > ignoreThreshold)
                    ReceiveMoveInput(delta);

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
                if (host.animator.GetBool(PlayerAnimeParams.move))
                    host.animator.SetBool(PlayerAnimeParams.move, false);

                Fall();
                host.combat.UpdateState();
                //host.combat.ShowHud(true);
            }
            else
            {
                if (!host.animator.GetBool(PlayerAnimeParams.move))
                    host.animator.SetBool(PlayerAnimeParams.move, true);
                var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
                host.cc.SimpleMove(deltaDist * speed);
                Rotate(deltaDist);

                host.interaction.HideAll();
                host.combat.ShowHud(false);
            }

            _moveDist = Vector2.zero;
        }

        void Fall()
        {
            if (!host.cc.isGrounded)
                host.cc.SimpleMove(-0.5f * Vector3.up);
        }

        public void Rotate(Vector3 to)
        {
            to.y = 0;
            if (to.magnitude <= 0)
                return;

            rotatePart.rotation = Quaternion.LookRotation(to);
        }

        public void ReceiveMoveInput(Vector2 dir)
        {
            _moveDist = dir.normalized;
        }
    }
}