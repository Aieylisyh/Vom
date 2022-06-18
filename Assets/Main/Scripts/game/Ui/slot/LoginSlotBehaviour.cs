using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class LoginSlotBehaviour : MonoBehaviour
    {
        public Image bg;
        public Sprite bgGreen;
        public Sprite bgBlue;
        public GameObject clearBtn;

        public RectTransform expBar;
        public Text levelTxt;
        public Text playedTxt;
        public Text lastPlayedTxt;
        public Text starCountTxt;
        private RectTransform _rectTransform;
        public GameObject levelView;
        private IRuntimeDataCache<AccountData> _accountDataCache;
        private IRuntimeDataCache<GameData> _gameDataCache;
        private int _index;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Setup(IRuntimeDataCache<AccountData> accountDataCache, IRuntimeDataCache<GameData> gameDataCache, int index)
        {
            //Debug.Log("Setup index " + index);
            //Debug.Log(accountDataCache);
            //Debug.Log(gameDataCache);

            _rectTransform.localScale = Vector3.zero;
            _accountDataCache = accountDataCache;
            _gameDataCache = gameDataCache;
            _index = index;
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        void SetEmpty()
        {
            levelView.SetActive(false);
            playedTxt.text = LocalizationService.instance.GetLocalizedText("NewGame");
            lastPlayedTxt.text = "";
            // levelTxt.text = LocalizationService.instance.GetLocalizedText("empty");
            expBar.localScale = new Vector3(0, 1, 1);
            bg.sprite = bgGreen;
            clearBtn.SetActive(false);
        }

        public void Refresh()
        {
            this.gameObject.SetActive(true);
            if (_gameDataCache.cache == null)
            {
                SetEmpty();
                return;
            }

            SetByData();
        }

        void SetByData()
        {
            clearBtn.SetActive(true);
            bg.sprite = bgBlue;
            var data = _gameDataCache.cache;
            levelTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("levelAny", "<size=120%>" + data.playerLevel + "</size>");

            var accountData = _accountDataCache.cache;
            var deltaPlayed = System.DateTime.Now - accountData.firstLaunchDate;
            var daysPlayed = deltaPlayed.TotalDays;
            string playedString = "";
            if (daysPlayed < 1)
            {
                playedString = LocalizationService.instance.GetLocalizedTextFormatted("PlayedTimeNoDay", deltaPlayed.Hours, deltaPlayed.Minutes, deltaPlayed.Seconds);
                //playedString = LocalizationService.instance.GetLocalizedTextFormatted("PlayedHours", deltaPlayed.Hours + "");
            }
            else
            {
                playedString = LocalizationService.instance.GetLocalizedTextFormatted("PlayedTime", Mathf.FloorToInt((float)daysPlayed), deltaPlayed.Hours, deltaPlayed.Minutes, deltaPlayed.Seconds);
                //playedString = LocalizationService.instance.GetLocalizedTextFormatted("PlayedDays", (int)daysPlayed + "");
            }
            var deltaLastPlayed = System.DateTime.Now - accountData.lastLaunchDate;
            var daysLastPlayed = deltaLastPlayed.TotalDays;
            string lastPlayedString = "";
            if (daysLastPlayed < 1)
            {
                //lastPlayedString = LocalizationService.instance.GetLocalizedText("LastPlayedToday");
                lastPlayedString = LocalizationService.instance.GetLocalizedTextFormatted("LastPlayedHoursAgo", deltaLastPlayed.Hours + "");
            }
            else
            {
                lastPlayedString = LocalizationService.instance.GetLocalizedTextFormatted("LastPlayedDaysAgo", (int)daysLastPlayed + "");
            }

            //var played = LocalizationService.instance.GetLocalizedTextFormatted("Played", playedString);
            var lastPlayed = LocalizationService.instance.GetLocalizedTextFormatted("LastPlayed", lastPlayedString);

            playedTxt.text = "<size=120%>" + playedString + "</size>";
            lastPlayedTxt.text = lastPlayed;

            var starCount = LevelService.instance.GetCampaignLevelStars(_gameDataCache);
            starCountTxt.text = starCount + "";
            var crt = data.exp;
            var max = 10000;

            float expRatio = (float)crt / max;
            expBar.localScale = new Vector3(expRatio, 1, 1);
            levelView.SetActive(true);
        }

        public void DoShowAnime()
        {
            _rectTransform.DOScale(1, 0.35f).SetEase(Ease.OutCubic);
        }

        public void DoResetAnime()
        {
            _rectTransform.localScale = Vector3.one * 0.5f;
            _rectTransform.DOScale(1, 0.35f).SetEase(Ease.OutCubic);
        }
        public void OnClickSlot()
        {
            LoginWindowBehaviour.instance.OnClickSlot(_index);
        }

        public void OnClickClear()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("ClearAccountTitle");
            data.content = LocalizationService.instance.GetLocalizedText("ClearAccountContent");
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");
            //data.btnRightTxt = LocalizationService.instance.GetLocalizedText("Cancel");
            data.btnLeftAction = ClearData;
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private void ClearData()
        {
            UxService.instance.ClearAccount(_index);
            LoginWindowBehaviour.instance.ShowPanel();
        }
    }
}
