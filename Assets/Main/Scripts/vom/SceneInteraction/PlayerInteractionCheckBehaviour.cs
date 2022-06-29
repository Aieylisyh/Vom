using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class PlayerInteractionCheckBehaviour : VomPlayerComponent
    {
        public List<SceneInteractionTargetBehaviour> interactionTargets;
        bool _canShow;

        bool _started;
        float _passedTimer;
        SceneInteractionTargetBehaviour _currentSi;

        public override void ResetState()
        {
            _canShow = true;
            interactionTargets = new List<SceneInteractionTargetBehaviour>();

            _passedTimer = 0;
            _started = false;
            _currentSi = null;
        }

        void Refresh()
        {
            foreach (var si in interactionTargets)
            {
                if (_canShow)
                {
                    if (_started)
                    {
                        if (si != _currentSi)
                        {
                            si.HideUi();
                        }
                        else
                        {
                            si.ShowUi();
                        }
                    }
                    else
                    {
                        si.ShowUi();
                    }
                }
                else
                {
                    si.HideUi();
                }
            }
        }

        public void HideAll()
        {
            //interrupted
            StopCurrentSi();
            _canShow = false;
            Refresh();
        }

        public void ShowAll()
        {
            //resume
            _canShow = true;
            Refresh();
        }

        public void Interact(SceneInteractionTargetBehaviour si)
        {
            _started = true;
            _currentSi = si;
            _passedTimer = 0;
            Refresh();
        }

        private void OnTriggerEnter(Collider other)
        {
            var si = other.GetComponent<SceneInteractionTargetBehaviour>();
            if (si != null)
            {
                Debug.Log("enter " + si.interaction);
                interactionTargets.Add(si);
                Refresh();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var si = other.GetComponent<SceneInteractionTargetBehaviour>();
            if (si != null)
            {
                Debug.Log("exit " + si.interaction);
                if (si == _currentSi)
                {
                    StopCurrentSi();
                }
                si.HideUi();
                interactionTargets.Remove(si);
                Refresh();
            }
        }

        void StopCurrentSi()
        {
            _currentSi = null;
            _started = false;
            _passedTimer = 0;
        }

        protected override void Update()
        {
            base.Update();

            if (_started)
            {
                var data = _currentSi.data;
                _passedTimer += GameTime.deltaTime;
                if (_passedTimer > data.duration)
                    _passedTimer = data.duration;

                _currentSi.ui.SyncProgress(_passedTimer / data.duration);
                if (_passedTimer == data.duration)
                    OnFinish();
            }
        }

        void OnFinish()
        {
            Debug.Log("OnFinish");
            _started = false;
        }
    }
}