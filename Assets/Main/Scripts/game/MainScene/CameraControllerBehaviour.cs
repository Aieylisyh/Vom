using UnityEngine;
//using System.Collections;
using com;
//using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

namespace game
{
    public class CameraControllerBehaviour : MonoBehaviour
    {
        public static CameraControllerBehaviour instance { get; private set; }
        public Transform dockPos1;
        public Transform dockPos2;
        public Transform dockPos3;
        public Camera combatCam;
        public Camera portCam;
        public Camera cinematicCam;
        private Transform _cineCamTarget;
        public Vector3 cinematicCamOffset;
        public Vector3 pediaOffset;
        //public PostProcessVolume ppv;
        //public PostProcessProfile ppp;
        //public PostProcessEffectSettings ppes;
        //public ColorGrading ppcg;
        public float leaveTime = 2f;
        public float enterTime = 3.5f;
        private float _timer;
        private float _time;
        public AnimationCurve ac;
        public float autoSwitchBackCineCamTime;
        private float _switchBackCineCamTimer;
        private Vector3 _offsetPortCam;
        private Vector3 _tpOffsetPortCam;
        private bool _targetIsPort;
        public bool currentIsPort { get; private set; }
        public GameObject[] portShow;
        public GameObject[] portHide;

        public float camTransitTime = 1;
        private float _camTransitTimer = 0;
        public float camTransitMaxLerp = 0.5f;
        private Vector3 _camTransitTargetPos;
        private Vector3 _camTransitStartPos;

        public Transform camPortPosStart;
        private Vector3 _combatCamPos;

        public Transform camCombatPosStart;
        public Transform camCombatPosEnd;
        Sequence camCombatSequence;

        public float camCombatSizeMin;
        public float combatCamDuration;
        public OthographicSizeMatchWidth combatMatcher;

        private void Awake()
        {
            instance = this;
            //ppv.profile.TryGetSettings(out ppcg);
        }

        private void Start()
        {
            _offsetPortCam = portCam.transform.position - MainSceneManager.instance.islandCenter.position;
            _targetIsPort = true;
            _combatCamPos = combatCam.transform.position;
        }

        public void OnStartGame()
        {
            SwitchToPortView();
            MoveShip(0);

            _tpOffsetPortCam = _offsetPortCam.normalized * 0.1f;
            _camTransitStartPos = camPortPosStart.position + _tpOffsetPortCam;
            _camTransitTargetPos = portCam.transform.position;
            portCam.transform.position = _camTransitStartPos;
            _camTransitTimer = camTransitTime;
        }

        public void EnterPort()
        {
            if (_targetIsPort)
            {
                return;
            }
            _targetIsPort = true;
            SceneTransitionBehaviour.instance.StartTransition();
        }

        public void LeavePort()
        {
            if (!_targetIsPort)
            {
                return;
            }
            _targetIsPort = false;
            _time = leaveTime;
            _timer = _time;
            SceneTransitionBehaviour.instance.StartTransition();
        }

        public void SwitchView()
        {
            if (_targetIsPort)
            {
                SwitchToPortView();
                GameFlowService.instance.EnqueueEvent(GameFlowService.GameFlowEvent.GoToPort, true);
            }
            else
            {
                SwitchToCombatView();
                GameFlowService.instance.EnqueueEvent(GameFlowService.GameFlowEvent.GoToCombat, true);
            }
        }

        public void SwitchToCinematicView(Transform target, float switchBackTimer = -1)
        {
            _cineCamTarget = target;

            cinematicCam.gameObject.SetActive(target != null);
            portCam.gameObject.SetActive(false);
            combatCam.gameObject.SetActive(target == null);
            SetSwitchBackTimer(switchBackTimer);
        }

        public void SetSwitchBackTimer(float switchBackTimer)
        {
            _switchBackCineCamTimer = switchBackTimer;
        }

        private void UpdateCinematic()
        {
            if (!cinematicCam.gameObject.activeSelf)
                return;

            if (_cineCamTarget == null || !_cineCamTarget.gameObject.activeInHierarchy)
            {
                if (_switchBackCineCamTimer == -1)
                {
                    _switchBackCineCamTimer = autoSwitchBackCineCamTime;
                    return;
                }
                if (_switchBackCineCamTimer > 0)
                {
                    _switchBackCineCamTimer -= Time.deltaTime;
                    if (_switchBackCineCamTimer <= 0)
                    {
                        _switchBackCineCamTimer = -1;
                        SwitchToCinematicView(null);
                    }
                }
                return;
            }

            var offset = _cineCamTarget.forward * cinematicCamOffset.x
                + _cineCamTarget.up * cinematicCamOffset.y
                + _cineCamTarget.right * cinematicCamOffset.z;
            cinematicCam.transform.position = _cineCamTarget.position + offset;
            cinematicCam.transform.rotation = Quaternion.LookRotation(_cineCamTarget.position - cinematicCam.transform.position, Vector3.up);
        }

        private void SwitchToPortView()
        {
            //black screen time
            currentIsPort = true;
            CombatService.instance.OnCombatExit();
            CombatService.instance.playerShip.move.transform.SetPositionAndRotation(dockPos3.position, dockPos3.rotation);

            _time = enterTime;
            _timer = _time;
            ResetPortCamTarget();

            foreach (var g in portHide)
            {
                g.SetActive(false);
            }
            foreach (var g in portShow)
            {
                g.SetActive(true);
            }
            portCam.gameObject.SetActive(true);
            combatCam.gameObject.SetActive(false);
        }

        private void SwitchToCombatView()
        {
            //black screen time
            currentIsPort = false;
            _timer = 0;
            CombatService.instance.OnCombatEnter();
            foreach (var g in portHide)
            {
                g.SetActive(true);
            }
            foreach (var g in portShow)
            {
                g.SetActive(false);
            }

            // camConbatMatcher.Match();
            combatCam.gameObject.SetActive(true);
            portCam.gameObject.SetActive(false);

            StartCombatViewAnim();
        }

        void StartCombatViewAnim()
        {
            if (LevelService.instance.IsPediaLevel())
            {
                StartPediaCombatCam();
                return;
            }

            StartNormalCombatCam();
        }

        void StartPediaCombatCam()
        {
            if (camCombatSequence != null)
                camCombatSequence.Kill();

            var cfg = ConfigService.instance.pediaConfig;
            combatCam.transform.SetPositionAndRotation(cfg.camPos, Quaternion.Euler(cfg.camEular));
            combatCam.orthographicSize = cfg.camSize - cfg.camSizeOffset;

            camCombatSequence = DOTween.Sequence();
            camCombatSequence.Append(combatCam.DOOrthoSize(cfg.camSize + cfg.camSizeOffset, cfg.camSizeDuration).SetEase(Ease.InOutQuad));
            camCombatSequence.Append(combatCam.DOOrthoSize(cfg.camSize - cfg.camSizeOffset, cfg.camSizeDuration).SetEase(Ease.InOutQuad));
            camCombatSequence.SetLoops(-1);
            camCombatSequence.Play();
        }

        void StartNormalCombatCam()
        {
            if (camCombatSequence != null)
                camCombatSequence.Kill();

            var trans = combatCam.transform;
            trans.SetPositionAndRotation(camCombatPosStart.position, camCombatPosStart.rotation);

            var sizeTo = combatMatcher.GetGoodSize();
            var sizeFrom = sizeTo * 0.6f;
            sizeFrom = Mathf.Max(camCombatSizeMin, sizeFrom);
            combatCam.orthographicSize = sizeFrom;

            trans.DORotate(camCombatPosEnd.eulerAngles, combatCamDuration).SetEase(Ease.InQuad);
            trans.DOMove(camCombatPosEnd.position, combatCamDuration).SetEase(Ease.InQuad);
            combatCam.DOOrthoSize(sizeTo, combatCamDuration).SetEase(Ease.InOutQuad);
        }

        void Update()
        {
            UpdateCinematic();
            UpdateCam();
            UpdateShip();
        }

        public void ResetPortCamTarget()
        {
            _tpOffsetPortCam = _offsetPortCam;
            portCam.transform.position = MainSceneManager.instance.islandCenter.position + _tpOffsetPortCam;
            _camTransitTimer = 0;
        }

        public void SetPortCamTarget(Vector3 pos, float distance = 0)
        {
            if (distance <= 0)
            {
                _tpOffsetPortCam = _offsetPortCam;
            }
            else
            {
                _tpOffsetPortCam = _offsetPortCam.normalized * distance;
            }

            pos = pos * camTransitMaxLerp + MainSceneManager.instance.islandCenter.position * (1 - camTransitMaxLerp);
            _camTransitTimer = camTransitTime;
            _camTransitTargetPos = pos + _tpOffsetPortCam;
            _camTransitStartPos = portCam.transform.position;
        }

        private void UpdateCam()
        {
            if (_camTransitTimer <= 0)
            {
                return;
            }

            _camTransitTimer -= Time.deltaTime;
            if (_camTransitTimer <= 0)
            {
                _camTransitTimer = 0;
            }
            float f = ac.Evaluate(_camTransitTimer / camTransitTime);
            MoveCam(f);
        }

        private void MoveCam(float f)
        {
            f = 1 - f;
            var pos = Vector3.Lerp(_camTransitStartPos, _camTransitTargetPos, f);
            portCam.transform.position = pos;
        }

        private void UpdateShip()
        {
            if (_timer <= 0)
            {
                return;
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = 0;
            }

            float f = ac.Evaluate(_timer / _time);
            MoveShip(f);
        }

        private void MoveShip(float f)
        {
            if (currentIsPort != _targetIsPort)
            {
                f = 1 - f;
            }
            //ppcg.brightness.value = -f * 100;
            // bezier
            CombatService.instance.playerShip.move.transform.SetPositionAndRotation(
                         MathGame.Lerp3Bezier(dockPos1.position, dockPos2.position, dockPos3.position, f),
                         MathGame.Lerp3Bezier(dockPos1.rotation, dockPos2.rotation, dockPos3.rotation, f));
        }

        public void ShakeCombatCam(float duration)
        {
            //Debug.Log("ShakeCombatCam");
            combatCam.transform.DOKill();

            combatCam.transform.DOShakePosition(duration, 0.6f, 8, 20).OnComplete(
                () =>
                {
                    combatCam.transform.position = _combatCamPos;
                }
                );
        }
    }
}