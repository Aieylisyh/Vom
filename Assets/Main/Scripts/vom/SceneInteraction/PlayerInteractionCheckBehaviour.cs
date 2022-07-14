using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class PlayerInteractionCheckBehaviour : VomPlayerComponent
    {
        public List<SceneInteractionTargetBehaviour> interactionTargets { get; private set; }
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
            for (int i = interactionTargets.Count - 1; i >= 0; i--)
            {
                var si = interactionTargets[i];
                if (si.triggered)
                {
                    interactionTargets.Remove(si);
                    continue;
                }

                if (_canShow)
                {
                    if (_started)
                    {
                        if (si != _currentSi)
                            si.HideUi();
                        else
                            si.ShowUi();
                    }
                    else
                        si.ShowUi();
                }
                else
                    si.HideUi();
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

        public void Sliced()
        {
            SoundService.instance.Play("slice");
            CameraShake.instance.Shake(CameraShake.ShakeLevel.VeryWeak);
            if (_currentSi != null)
            {
                switch (_currentSi.interaction)
                {
                    case ESceneInteraction.None:
                    case ESceneInteraction.Fruit:
                    case ESceneInteraction.Chest:
                    case ESceneInteraction.Fish:
                    case ESceneInteraction.Herb:
                        return;

                    case ESceneInteraction.Tree:
                        (_currentSi as FruitTreeBehaviour).SliceFeedback();
                        return;

                    case ESceneInteraction.Mine:
                        (_currentSi as MineBehaviour).SliceFeedback();
                        break;

                    case ESceneInteraction.Dig:
                        (_currentSi as DigHoleBehaviour).SliceFeedback();
                        break;
                }
            }
        }

        public void Interact(SceneInteractionTargetBehaviour si)
        {
            host.move.Rotate(si.transform.position - transform.position);
            if (!si.TestCanInteract())
                return;

            _started = true;
            _currentSi = si;
            _passedTimer = 0;
            Refresh();
            host.animator.ResetTrigger(PlayerAnimeParams.stopSlice);

            switch (si.interaction)
            {
                case ESceneInteraction.Chest:
                    host.animator.SetTrigger(PlayerAnimeParams.jump);
                    break;

                case ESceneInteraction.Fruit:
                    host.animator.SetTrigger(PlayerAnimeParams.slice);
                    break;

                case ESceneInteraction.Tree:
                    host.animator.SetTrigger(PlayerAnimeParams.slice);
                    break;

                case ESceneInteraction.Dig:
                    host.animator.SetTrigger(PlayerAnimeParams.slice);
                    break;

                case ESceneInteraction.Mine:
                    host.animator.SetTrigger(PlayerAnimeParams.slice);
                    break;

                case ESceneInteraction.Herb:
                    host.animator.SetTrigger(PlayerAnimeParams.slice);
                    break;

                case ESceneInteraction.Fish:
                    host.animator.SetTrigger(PlayerAnimeParams.jump);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var si = other.GetComponent<SceneInteractionTargetBehaviour>();
            if (si != null && !si.triggered)
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
                //Debug.Log("exit " + si.interaction);
                if (si == _currentSi)
                    StopCurrentSi();

                si.HideUi();
                interactionTargets.Remove(si);
                Refresh();
            }
        }

        void StopCurrentSi()
        {
            if (_started)
            {
                _currentSi = null;
                _started = false;
                _passedTimer = 0;

                StopConsistInteractionAnim();
            }
        }

        void StopConsistInteractionAnim()
        {
            var i = host.animator.GetCurrentAnimatorClipInfo(0);
            if (i.Length == 1 && i[0].clip.name == "slice")
            {
                host.animator.SetTrigger(PlayerAnimeParams.stopSlice);
            }
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
            _currentSi.OnFinish();
            _started = false;
            StopConsistInteractionAnim();
        }
    }
}