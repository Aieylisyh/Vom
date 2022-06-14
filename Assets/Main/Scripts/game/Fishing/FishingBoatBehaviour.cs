using UnityEngine;
using com;
using System.Collections.Generic;
using DG.Tweening;

namespace game
{
    public class FishingBoatBehaviour : Ticker
    {
        public static FishingBoatBehaviour instance;
        public Transform portPos;
        public Transform midPos;
        public Transform oceanPos;

        private bool _currentIsPort;
        private bool _targetIsInPort;

        public float leaveTime = 3.5f;
        public float enterTime = 5f;

        private float _timer;
        private float _time;

        public AnimationCurve ac;
        public MoveSinBehaviour msb;
        public List<BoatLevelupPartViewBehaviour> partViews;

        public MoveMaterialTiling mmt;

        public GameObject leaveCurlGo;
        public MainSceneRftTimer mainSceneRftTimer;
        public ParticleSystem curlPs;

        private void ShowCurl()
        {
            if (!leaveCurlGo.activeSelf)
            {
                leaveCurlGo.transform.localScale = Vector3.zero;
                leaveCurlGo.SetActive(true);
                leaveCurlGo.transform.DOScale(1, 1.0f);
            }

            mainSceneRftTimer.gameObject.SetActive(true);
            mainSceneRftTimer.ForceTick();
            curlPs.Play(true);
        }

        private void HideCurl()
        {
            if (leaveCurlGo.activeSelf)
            {
                curlPs.Play(true);
                leaveCurlGo.transform.DOScale(0, 1.0f).OnComplete(
                    () =>
                    {
                        mainSceneRftTimer.gameObject.SetActive(false);
                        leaveCurlGo.SetActive(false);
                    });
            }
        }

        private void Awake()
        {
            instance = this;

            foreach (var p in partViews)
            {
                p.Init();
            }
        }

        protected override void Tick()
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main)
            {
                if (!_targetIsInPort && !_currentIsPort)
                {
                    if (!FishingService.instance.HasRft())
                        return;

                    //Debug.Log("boat tick");
                    var timerValue = FishingService.instance.GetRftRestTimeSpan();
                    if (timerValue.Ticks <= 0)
                    {
                        EnterPort();
                        FishingWindowBehaviour.instance.OnBoatArriving();
                    }
                }
            }
        }

        public void UpdatePartViews()
        {
            var level = FishingService.instance.GetBoatLevel();
            //Debug.Log("UpdatePartViews " + level);
            foreach (var p in partViews)
            {
                var covered = p.correspondingBoatLevel <= level;
                if (covered)
                {
                    p.Show();
                }
                else
                {
                    p.Hide();
                }
            }
        }

        public void ShowPartViewLevelupTarget()
        {
            var level = FishingService.instance.GetBoatLevel();
            foreach (var p in partViews)
            {
                if (p.correspondingBoatLevel == level + 1)
                {
                    p.Show();
                    p.StartAnimate();
                }
            }
        }

        public void StopPartViews()
        {
            var level = FishingService.instance.GetBoatLevel();
            foreach (var p in partViews)
            {
                p.StopAnimate();
            }
        }

        public void StopAllParts()
        {
            foreach (var p in partViews)
            {
                p.StopAnimate();
            }
        }

        public bool IsAvailable()
        {
            if (_targetIsInPort && _currentIsPort)
            {
                return true;
            }

            return false;
        }

        public bool IsInOcean()
        {
            if (!_targetIsInPort)
            {
                return true;
            }

            return false;
        }

        public bool IsArriving()
        {
            if (_targetIsInPort && !_currentIsPort)
            {
                return true;
            }

            return false;
        }

        public void SetInPort()
        {
            //Debug.Log("SetInPort");
            _currentIsPort = true;
            _targetIsInPort = true;
            transform.SetPositionAndRotation(portPos.position, portPos.rotation);
            _timer = 0;
            //msb.enabled = true;
            UpdatePartViews();

            HideCurl();
        }

        public void SetInOcean()
        {
            //Debug.Log("SetInOcean");
            _currentIsPort = false;
            _targetIsInPort = false;
            transform.SetPositionAndRotation(oceanPos.position, oceanPos.rotation);
            _timer = 0;

            ShowCurl();
            //msb.enabled = false;
        }

        public void LeavePort()
        {
            if (!_currentIsPort)
                return;

            //Debug.Log("LeavePort");
            SetInPort();
            _targetIsInPort = false;

            _time = leaveTime;
            _timer = _time;

            //msb.enabled = false;
        }

        public void EnterPort()
        {
            if (_currentIsPort)
                return;

            Debug.Log("EnterPort");
            SetInOcean();
            UpdatePartViews();

            _targetIsInPort = true;
            _time = leaveTime;
            _timer = _time;

            HideCurl();
            //msb.enabled = false;
        }

        protected override void Update()
        {
            base.Update();
            UpdateBoat();
        }

        private void UpdateBoat()
        {
            if (_timer <= 0)
            {
                return;
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = 0;
                //Debug.Log("Move Boat end _targetIsInPort " + _targetIsInPort);
                //Debug.LogWarning("now " + System.DateTime.Now.Ticks);

                if (_targetIsInPort)
                {
                    SetInPort();
                    FishingWindowBehaviour.instance.OnBoatArrived();
                }
                else
                {
                    SetInOcean();
                }
                return;
            }

            float f = ac.Evaluate(_timer / _time);
            //Debug.Log("MoveBoat " + _timer + "/" + _time + " f " + f);
            MoveBoat(f);
        }

        private void MoveBoat(float f)
        {
            //Debug.Log("MoveBoat" + f);
            if (!_targetIsInPort)
            {
                f = 1 - f;
            }

            var pos = MathGame.Lerp3Bezier(portPos.position, midPos.position, oceanPos.position, f);
            var rot = MathGame.Lerp3Bezier(portPos.rotation, midPos.rotation, oceanPos.rotation, f);
            //Debug.Log(pos);
            transform.SetPositionAndRotation(pos, rot);
        }
    }
}