using UnityEngine;
using DG.Tweening;
using com;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

namespace vom
{
    public class MapFeedbackSystem : MonoBehaviour
    {
        public CanvasGroup cg;
        public TMPro.TextMeshProUGUI text;

        Color _crtColor;
        Color _targetColor;
        public float colorTime;
        float _colorTimer;

        public float txtTime;
        float _txtTimer;
        public AnimationCurve textAc;

        public void EnterNewMap()
        {
            var map = MapSystem.instance.currentMap;
            SetBgColor(map.prototype.bgColor);
            ShowMapTitle(map.prototype.title);
        }

        void SetBgColor(Color c)
        {
            _crtColor = RenderSettings.fogColor;
            _targetColor = c;
            _colorTimer = colorTime;
        }

        void ShowMapTitle(string t)
        {
            text.text = t;
            _txtTimer = txtTime;
            cg.alpha = 0;
        }

        private void Update()
        {
            if (_txtTimer > 0)
            {
                _txtTimer -= GameTime.deltaTime;
                if (_txtTimer < 0)
                    _txtTimer = 0;
                var r = textAc.Evaluate(_txtTimer / txtTime);
                cg.alpha = r;
            }

            if (_colorTimer > 0)
            {
                _colorTimer -= GameTime.deltaTime;
                if (_colorTimer < 0)
                    _colorTimer = 0;
                var c = Color.Lerp(_crtColor, _targetColor, 1 - _colorTimer / colorTime);
                RenderSettings.fogColor = c;
            }
        }
    }
}