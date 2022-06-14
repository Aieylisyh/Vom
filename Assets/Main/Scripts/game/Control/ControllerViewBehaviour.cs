using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

namespace game
{
    public class ControllerViewBehaviour : MonoBehaviour
    {
        public GameObject ArrowView;
        public GameObject RingView;
        public RectTransform icon;
        public RectTransform ArrowRotateTrans;
        public RectTransform ArrowSizeTrans;
        public RectTransform RingRotateTrans;
        public RectTransform RingSizeTrans;

        public float widthArrow = 60;
        public float arrowSizeFactor = 1.00f;
        public float sizeMax = 450;
        public float sizeMin = 320f;

        public float ringSizeFactor = 1.25f;
        public GameObject TorDirectionalView;
        public GameObject TorTraceView;

        public Image progressBar;
        public CanvasGroup chargingProgressCg;
        public RectTransform chargingProgress;
        public ParticleSystem psCharging;
        public CanvasGroup chargedRingCg;
        public RectTransform chargedRing;
        public float chargeProgressDisplayPercent = 0.15f;

        //public RectTransform rect;
        //public float offsetY = -10;
        //public float offsetX = 0;
        private void Start()
        {
            UpdateCharged(0);
            HideDirectional();
        }

        public void UpdateCharged(float v)
        {
            //dont delete this
            //这把一个ui放在船的位置上，而不是用的parent的方法，但是目前用不着了
            //var pp = CameraController.instance.combatCam.WorldToScreenPoint(CombatService.instance.playerShip.move.transform.position);
            //pp += new Vector3(offsetX, offsetY, 0);
            //pp /= InputPanel.instance.canvasScale;
            //rect.anchoredPosition = pp;

            if (v <= chargeProgressDisplayPercent)
            {
                //HideCharged
                if (chargingProgressCg.alpha != 0)
                {
                    chargingProgress.DOKill();
                    chargingProgressCg.alpha = 0;
                    chargedRingCg.DOKill();
                    chargedRingCg.alpha = 0;
                    ShowIcon();
                    psCharging.Stop(true);
                }
            }
            else if (v >= 1)
            {
                //ShowCharged
                if (progressBar.fillAmount < 1)
                {
                    psCharging.Stop(true);

                    ShowIcon(true, false);
                    chargedRingCg.DOKill();
                    SetProgress(1);
                    chargedRing.localScale = Vector3.one * 3.5f;
                    chargedRing.DOScale(1, 0.35f);
                    chargedRingCg.alpha = 0;
                    chargedRingCg.DOFade(1, 0.35f);

                    chargingProgress.DOKill();
                    chargingProgress.DOPunchScale(Vector3.one * 1.3f, 0.35f, 1, 0.5f);
                    chargingProgressCg.alpha = 1;
                }
            }
            else
            {
                //charging progressing
                if (chargingProgressCg.alpha != 1)
                {
                    psCharging.Play(true);
                    chargingProgressCg.alpha = 1;
                    chargingProgress.DOKill();
                }

                chargingProgress.localScale = Vector3.one * (v * 0.5f + 0.75f);
                SetProgress(v);
            }
        }

        void SetProgress(float v)
        {
            progressBar.fillAmount = v;
            progressBar.color = Color.Lerp(new Color(0, 0.5f, 0.5f), Color.white, v);
        }

        public void ShowIcon(bool torTrace = false, bool torDir = false)
        {
            //Debug.Log(torTrace + "  " + torDir);
            bool dotween = false;
            if (torTrace && !TorTraceView.activeSelf)
            {
                TorTraceView.SetActive(true);
                dotween = true;
            }
            else if (!torTrace && TorTraceView.activeSelf)
            {
                TorTraceView.SetActive(false);
            }

            if (torDir && !TorDirectionalView.activeSelf)
            {
                TorDirectionalView.SetActive(true);
                ArrowView.SetActive(true);
                RingView.SetActive(true);
                dotween = true;
            }
            else if (!torDir && TorDirectionalView.activeSelf)
            {
                TorDirectionalView.SetActive(false);
                ArrowView.SetActive(false);
                RingView.SetActive(false);
            }
            if (dotween)
            {
                icon.localScale = Vector3.one;
                icon.DOKill();
                icon.DOPunchScale(Vector3.one * 1.2f, 0.35f, 1, 0.6f);
            }
        }

        public void HideDirectional()
        {
            ArrowView.SetActive(false);
            RingView.SetActive(false);

            ShowIcon(false);
        }

        public void ShowDirectional(Vector2 vec2)
        {
            //Debug.Log(vec2);
            ShowIcon(false, true);
            float sizeFactor = vec2.magnitude;
            float rotDeg = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
            // Debug.Log("--ShowDirectional " + vec2);
            //Debug.Log("sizeFactor " + sizeFactor + " rotDeg " + rotDeg);

            ArrowView.SetActive(true);
            RingView.SetActive(true);
            float sizefactorScaled = sizeFactor * 0.5f + sizeMin - 10;
            //ArrowPosTrans.anchoredPosition = new Vector2(0, -sizeFactor);
            ArrowRotateTrans.localEulerAngles = new Vector3(0, 0, rotDeg - 90);
            var goodSize = Mathf.Min(sizeMax, Mathf.Max(sizeMin, sizefactorScaled));
            ArrowSizeTrans.sizeDelta = new Vector2(widthArrow, arrowSizeFactor * goodSize);

            RingRotateTrans.localEulerAngles = new Vector3(0, 0, rotDeg);
            var sizeRing = ringSizeFactor * goodSize;
            RingSizeTrans.sizeDelta = new Vector2(sizeRing, sizeRing);
        }
    }
}