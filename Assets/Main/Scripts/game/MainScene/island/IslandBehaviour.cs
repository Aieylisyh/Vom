using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class IslandBehaviour : MonoBehaviour
    {
        public static List<IslandBehaviour> islands = new List<IslandBehaviour>();
        public Outline outline;
        protected Vector3 _disappearPos;
        public Transform camTarget;

        //public bool clickDisappearOther = true;
        public bool disableAppear = false;
        public float appearDistance = 50;
        public float appearTime = 2.5f;
        private float _appearRestTimer;
        private bool _targetStatusAppeared;
        private bool _currentStatusAppeared;
        private Vector3 _startPos;
        public float distance = 0;//relatively

        private void Awake()
        {
            islands.Add(this);
            //outline = GetComponent<Outline>();
            _startPos = transform.position;
            _currentStatusAppeared = true;
            _targetStatusAppeared = true;
        }

        private void Start()
        {
            Vector3 dir = _startPos - MainSceneManager.instance.islandCenter.position;
            dir.y = 0;
            _disappearPos = _startPos + dir.normalized * appearDistance;
        }

        public void OnClicked()
        {
            //Debug.Log("ClickOn " + gameObject);
            CameraControllerBehaviour.instance.SetPortCamTarget(camTarget == null ? transform.position : camTarget.position, distance);
            ClickFunction();
            //if (clickDisappearOther)
            //{
            //    DisappearOtherIslands();
            //}
            //ClickFunction();
            //ShowOutline();
        }

        protected void DisappearOtherIslands()
        {
            MainSceneManager.instance.DisappearOtherIslands(this);
        }

        public virtual void ClickFunction()
        {
            //Debug.Log("PlayIslandBehaviour!");
        }

        public void SetOutlineThin()
        {
            if (outline == null)
                return;
            outline.OutlineWidth = 1;
        }
        public void SetOutlineThick()
        {
            if (outline == null)
                return;
            outline.OutlineWidth = 6;
        }
        public void SetOutlineNone()
        {
            if (outline == null)
                return;
            outline.OutlineWidth = 0;
        }

        public void Appear(bool hardReset)
        {
            //Debug.Log("Appear");
            if (disableAppear)
            {
                return;
            }
            if (hardReset)
            {
                _appearRestTimer = appearTime;
            }
            else
            {
                if (_targetStatusAppeared && !_currentStatusAppeared)
                {
                    //appearing
                    //do nothing to keep _appearRestTimer value;
                }
                else if (!_targetStatusAppeared && _currentStatusAppeared)
                {
                    //disappearing
                    _appearRestTimer = appearTime - _appearRestTimer;
                }
                else if (_targetStatusAppeared && _currentStatusAppeared)
                {
                    //in
                    //do nothing to keep _appearRestTimer value;
                }
                else
                {
                    //out
                    _appearRestTimer = appearTime;
                }
            }

            _targetStatusAppeared = true;
            _currentStatusAppeared = false;
        }

        public void Disappear(bool hardReset)
        {
            //Debug.Log("Disappear");
            if (disableAppear)
            {
                return;
            }
            if (hardReset)
            {
                _appearRestTimer = appearTime;
            }
            else
            {
                if (_targetStatusAppeared && !_currentStatusAppeared)
                {
                    //appearing
                    _appearRestTimer = appearTime - _appearRestTimer;
                }
                else if (!_targetStatusAppeared && _currentStatusAppeared)
                {
                    //disappearing
                    //do nothing to keep _appearRestTimer value;  
                }
                else if (_targetStatusAppeared && _currentStatusAppeared)
                {
                    //in
                    _appearRestTimer = appearTime;
                }
                else
                {
                    //out
                    //do nothing to keep _appearRestTimer value;
                }
            }

            _targetStatusAppeared = false;
            _currentStatusAppeared = true;
        }

        private void Update()
        {
            if (disableAppear)
            {
                return;
            }
            DoAppear();
        }

        private void DoAppear()
        {
            if (_targetStatusAppeared == _currentStatusAppeared)
            {
                return;
            }
            float f;
            float fAc;
            f = _appearRestTimer / appearTime;

            _appearRestTimer -= com.GameTime.deltaTime;
            if (_appearRestTimer <= 0)
            {
                _appearRestTimer = 0;
                f = 0;
                _currentStatusAppeared = _targetStatusAppeared;
            }

            if (_targetStatusAppeared)
            {
                fAc = MainSceneManager.instance.acAppear.Evaluate(1 - f);
            }
            else
            {
                fAc = MainSceneManager.instance.acAppear.Evaluate(f);
            }

            transform.position = Vector3.LerpUnclamped(_disappearPos, _startPos, fAc);
        }
    }
}
