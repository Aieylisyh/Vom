using UnityEngine;
using DG.Tweening;
using com;
using System;
using Text = TMPro.TextMeshProUGUI;
using System.Collections.Generic;

namespace game
{
    public class RoundEndWindowBehaviour : WindowBehaviour
    {
        public RectTransform txtWin;
        public RectTransform txtLoose;
        public Text levelTitleText;

        public RectTransform iconWin;
        public RectTransform iconLoose;
        public float iconYEnd;
        public float iconYStart;

        public RectTransform dynamicPanel;
        public float dynamicPanelW_exp;
        public float dynamicPanelH_exp;
        public float dynamicPanelW_score;
        public float dynamicPanelH_score;
        public float dynamicPanelW_loots;
        public float dynamicPanelH_loots;

        public float inPanelTextsAnchoredYBase;
        public float inPanelTextsAnchoredYAdd;

        public RectTransform expTexts;
        public Text expMainText;
        public Text expDescText;

        public RectTransform lootsTexts;
        public Text lootsMainText;
        public Text lootsDescText;

        public RectTransform scoreTexts;
        public Text scoreMainText;
        public Text scoreDescText;

        public RectTransform stars;
        public RectTransform star1;
        public RectTransform star2;
        public RectTransform star3;
        public ParticleSystem psStar1;
        public ParticleSystem psStar2;
        public ParticleSystem psStar3;
        public RectTransform txtHighScore;

        public ParticleSystem psExp;
        public ParticleSystem psScore;
        public ParticleSystem psWin;
        public ParticleSystem psLoose;

        public float expFlowInterval = 0.1f;
        public float starFlowInterval = 0.4f;
        public int expFlowCount = 10;
        private int _expFlowCounter = 0;
        private int _expAddRest = 0;
        private int _expAddPerFlow = 0;
        private int _starFlowCounter = 0;

        public WindowInventoryBehaviour wib;
        public float penalSwitchInterval = 0.7f;
        public GameObject nextLevelButton;
        public GameObject quitButton;

        public static RoundEndWindowBehaviour instance;

        private Action _nextAction;
        private bool _winOrLoose;
        private int _score;
        private int _exp;
        private int _starCount;
        private int _starRestCount;
        private List<Item> _pureLoots;

        private int _levelBeforeExpAdd;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public void ShowRoundEnd(bool winOrLoose)
        {
            //Debug.LogWarning("ShowRoundEnd win " + winOrLoose);
            _winOrLoose = winOrLoose;
            TutorialService.instance.RecordLevelPlayed(_winOrLoose);
            Setup();
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }

        public override void Show()
        {
            //Debug.LogError("should not call this!");
        }

        public override void Setup()
        {
            //Debug.Log("RoundEndWindowBehaviour Setup");
            LevelHudBehaviour.instance.Hide();
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Paused);
            ResetState();
            StartAnime();
        }

        private void HideInPanelTxts()
        {
            expMainText.transform.gameObject.SetActive(false);
            expDescText.transform.gameObject.SetActive(false);
            lootsMainText.transform.gameObject.SetActive(false);
            lootsDescText.transform.gameObject.SetActive(false);
            scoreMainText.transform.gameObject.SetActive(false);
            scoreDescText.transform.gameObject.SetActive(false);
        }

        private void ResetInPanelTextsPos()
        {
            var pos = new Vector2(0, inPanelTextsAnchoredYBase);
            expTexts.anchoredPosition = pos;
            scoreTexts.anchoredPosition = pos;
            lootsTexts.anchoredPosition = pos;
        }

        private void ResetState()
        {
            MainHudBehaviour.instance.Hide();
            _nextAction = null;
            dynamicPanel.sizeDelta = Vector2.zero;
            levelTitleText.transform.gameObject.SetActive(false);
            txtWin.transform.gameObject.SetActive(false);
            txtLoose.transform.gameObject.SetActive(false);
            HideInPanelTxts();
            ResetInPanelTextsPos();
            txtHighScore.transform.gameObject.SetActive(false);
            nextLevelButton.SetActive(false);
            quitButton.SetActive(false);
            iconWin.transform.gameObject.SetActive(false);
            iconLoose.transform.gameObject.SetActive(false);
            iconWin.anchoredPosition = new Vector2(0, iconYStart);
            iconLoose.anchoredPosition = new Vector2(0, iconYStart);
            _pureLoots = new List<Item>();
            _exp = 0;
            wib.Clear();
            psExp.Stop(true);
            psScore.Stop(true);
            psWin.Stop(true);
            psLoose.Stop(true);
            _expFlowCounter = 0;
            _expAddRest = 0;
            _expAddPerFlow = 0;
            _starFlowCounter = 0;

            stars.transform.gameObject.SetActive(false);
            star1.transform.gameObject.SetActive(false);
            star2.transform.gameObject.SetActive(false);
            star3.transform.gameObject.SetActive(false);
            psStar1.Stop(true);
            psStar2.Stop(true);
            psStar3.Stop(true);
            _starCount = 0;
            _starRestCount = 0;
            //_score = 0;
        }

        private void StartAnime()
        {
            var runtimeLevel = LevelService.instance.runtimeLevel;

            foreach (var loot in runtimeLevel.totalLoot)
            {
                if (loot.id == "Exp")
                {
                    _exp = loot.n;
                    continue;
                }
                _pureLoots.Add(loot);
            }

            _score = runtimeLevel.score;
            string pLevelName = runtimeLevel.levelProto.title;
            levelTitleText.text = LocalizationService.instance.GetLocalizedText(pLevelName);

            _nextAction = ShowRoundEndTxt;
            _nextAction?.Invoke();
        }

        private void ScaleShow(Transform trans, bool hasNextCb, float time = 0.35f, float delay = 0)
        {
            //Debug.Log("ScaleShow " + trans);
            SoundService.instance.Play("tuto");
            trans.gameObject.SetActive(true);
            trans.localScale = Vector3.zero;
            if (hasNextCb)
                trans.DOScale(1, time).SetEase(Ease.OutCubic).SetDelay(delay).OnComplete(() => { _nextAction?.Invoke(); });
            else
                trans.DOScale(1, time).SetEase(Ease.OutCubic).SetDelay(delay);
        }

        private void ShowRoundEndTxt()
        {
            SoundService.instance.Play((_winOrLoose ? "win" : "loose"));
            (_winOrLoose ? psWin : psLoose).Play(true);
            ScaleShow(levelTitleText.transform, false);
            _nextAction = ShowIconDrop;
            ScaleShow((_winOrLoose ? txtWin : txtLoose).transform, true, 0.5f);
        }

        private void ShowIconDrop()
        {
            var icon = _winOrLoose ? iconWin : iconLoose;
            icon.transform.gameObject.SetActive(true);
            _nextAction = ShowPanelExp;

            icon.DOAnchorPosY(iconYEnd, 0.35f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                var crtLevelItem = LevelService.instance.runtimeLevel.levelItem;
                if (!crtLevelItem.passed && _winOrLoose)
                {
                    ShowFirstPassFreeChallenge();
                }
                else
                {
                    _nextAction?.Invoke();
                }
            });
        }

        void ShowFirstPassFreeChallenge()
        {
            SoundService.instance.Play("btn big");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_FpdcTitle");
            data.content = LocalizationService.instance.GetLocalizedText("MNI_FpdcContent");
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            data.btnLeftAction = () =>
            {
                _nextAction?.Invoke();
            };
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private void ShowPanelExp()
        {
            //Debug.Log("ShowPanelExp");
            if (_exp <= 0)
            {
                ShowPanelScore();
                return;
            }

            dynamicPanel.DOSizeDelta(new Vector2(dynamicPanelW_exp, dynamicPanelH_exp), 0.35f, true).OnComplete(
           () =>
           {
               var percent = getExpBonusPercent();
               int bonusValue = MathGame.GetPercentage(_exp, percent);
               var mainString = _exp + "";
               if (bonusValue > 0)
               {
                   mainString += " +" + bonusValue;
                   var descString = percent + "%";
                   expDescText.text = LocalizationService.instance.GetLocalizedTextFormatted("RDED_expDesc", descString);
                   ScaleShow(expDescText.transform, false, 0.35f, 0.3f);
               }

               _expFlowCounter = expFlowCount;
               _expAddRest = _exp + bonusValue;
               _expAddPerFlow = _expAddRest / expFlowCount;

               _nextAction = StartAddExp;
               expMainText.text = LocalizationService.instance.GetLocalizedTextFormatted("RDED_expMain", mainString);
               ScaleShow(expMainText.transform, true);
           });
        }

        private void StartAddExp()
        {
            psExp.Play(true);
            _nextAction = EndAddExp;
            _levelBeforeExpAdd = UxService.instance.gameDataCache.cache.playerLevel;
            MainHudBehaviour.instance.RefreshToDefault();

            var tweenSequence = DOTween.Sequence();
            for (int i = 0; i < expFlowCount - 1; i++)
            {
                tweenSequence.AppendInterval(expFlowInterval);
                tweenSequence.AppendCallback(() =>
                {
                    _expAddRest -= _expAddPerFlow;
                    AddExp(_expAddPerFlow);
                });
            }
            tweenSequence.AppendInterval(expFlowInterval);
            tweenSequence.AppendCallback(() =>
            {
                AddExp(_expAddRest);
                psExp.Stop(true);
            });
            tweenSequence.AppendInterval(penalSwitchInterval);
            tweenSequence.AppendCallback(() =>
            {
                _nextAction?.Invoke();
            });

            tweenSequence.Play();
        }

        private List<Item> GetLevelupRewards(int lvFrom)
        {
            var res = new List<Item>();
            res.Add(new Item(1, "TalentPoint"));
            return res;
        }

        private void EndAddExp()
        {
            //Debug.Log("EndAddExp");
            var crtLevel = UxService.instance.gameDataCache.cache.playerLevel;
            var levelupCount = crtLevel - _levelBeforeExpAdd;
            _nextAction = ShowPanelScore;
            if (levelupCount < 1)
            {
                _nextAction?.Invoke();
                return;
            }

            SoundService.instance.Play("ship unlock");

            var rewards = new List<Item>();
            for (var lvFrom = _levelBeforeExpAdd; lvFrom < crtLevel; lvFrom++)
            {
                var lvupRewards = GetLevelupRewards(lvFrom);
                //Debug.Log(lvupRewards.Count);
                //Debug.Log("lv up rwd lvFrom " + lvFrom);
                //Debug.Log(lvupRewards[0].id);
                //Debug.Log(lvupRewards[0].n);
                rewards = ItemService.instance.MergeItems(lvupRewards, rewards);
            }

            //TextFormat.LogObj(rewards);
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnLeftAction = () =>
            {
                foreach (var reward in rewards)
                    UxService.instance.AddItem(reward);

                SoundService.instance.Play(new string[3] { "reward", "pay1", "pay2" });
                var itemsData = new ItemsPopup.ItemsPopupData();
                itemsData.clickBgClose = false;
                itemsData.hasBtnOk = true;
                itemsData.title = LocalizationService.instance.GetLocalizedText("PlayerLevelupRewardTitle");
                itemsData.content = LocalizationService.instance.GetLocalizedText("PlayerLevelupRewardContent");
                itemsData.items = rewards;
                itemsData.OkCb = _nextAction;
                WindowService.instance.ShowItemsPopup(itemsData);
            };

            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("PlayerLevelupTitle");
            data.content = LocalizationService.instance.GetLocalizedTextFormatted("PlayerLevelupContent", crtLevel + "");
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Awesome");

            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private void AddExp(int v)
        {
            //Debug.Log("AddExp " + v + " " + Time.time);
            SoundService.instance.Play("tuto");
            UxService.instance.AddItem("Exp", v);
        }

        private void ShowPanelScore()
        {
            //Debug.Log("ShowPanelScore");
            MainHudBehaviour.instance.Hide();
            if (!_winOrLoose || _score <= 0)
            {
                ShowPanelLoots();
                return;
            }

            if (expMainText.transform.gameObject.activeSelf)
            {
                //move up expMainText
                expTexts.DOAnchorPosY(inPanelTextsAnchoredYAdd + expTexts.anchoredPosition.y, 0.35f, true);
            }

            dynamicPanel.DOSizeDelta(new Vector2(dynamicPanelW_score, dynamicPanelH_score), 0.35f, true).OnComplete(
           () =>
           {
               var rtlProto = LevelService.instance.runtimeLevel.levelProto;

               var maxScore = LevelService.instance.runtimeLevel.maxScore;
               var cfg = ConfigService.instance.levelConfig;
               var star1Score = 0;
               var star2Score = cfg.score2StarRatio * maxScore;
               var star3Score = cfg.score3StarRatio * maxScore;
               _starCount = 0;
               if (_score >= star3Score)
                   _starCount = 3;
               else if (_score >= star2Score)
                   _starCount = 2;
               else if (_score >= star1Score)
                   _starCount = 1;
               //Debug.Log("Stars " + _starCount + " maxScore " + maxScore + " gaps:" + star3Score + "/" + star2Score + "/" + star1Score);
               _starRestCount = _starCount;
               var mainString = _score + "";
               if (_starCount < 3)
               {
                   var descString = maxScore + "";
                   scoreDescText.text = LocalizationService.instance.GetLocalizedTextFormatted("RDED_scoreDesc", descString);
                   ScaleShow(scoreDescText.transform, false, 0.35f, 0.3f);
               }

               _starFlowCounter = 1 + _starCount;

               _nextAction = StartAddStar;
               scoreMainText.text = LocalizationService.instance.GetLocalizedTextFormatted("RDED_scoreMain", mainString);
               ScaleShow(scoreMainText.transform, true);
           });
        }

        private void StartAddStar()
        {
            psScore.Play(true);
            _nextAction = EndAddStar;

            var tweenSequence = DOTween.Sequence();
            for (int i = 0; i < _starFlowCounter; i++)
            {
                tweenSequence.AppendInterval(starFlowInterval);
                tweenSequence.AppendCallback(() =>
                {
                    AddStar();
                });
            }

            tweenSequence.AppendInterval(starFlowInterval);
            tweenSequence.AppendCallback(() =>
           {
               psScore.Stop(true);
           });
            tweenSequence.AppendInterval(penalSwitchInterval);
            tweenSequence.AppendCallback(() =>
            {
                _nextAction?.Invoke();
            });
            tweenSequence.Play();
        }

        private void AddStar()
        {
            var flowIndex = _starCount - _starRestCount;
            _starRestCount--;
            //Debug.Log("Add starRestCount " + _starRestCount);
            if (flowIndex == 0)
            {
                //first empty flow
                ScaleShow(stars.transform, false);
            }
            else if (flowIndex < 4)
            {
                SoundService.instance.Play("notif");
                if (flowIndex == 1)
                {
                    ScaleShow(star1.transform, false);
                    psStar1.Play(true);
                }
                else if (flowIndex == 2)
                {
                    ScaleShow(star2.transform, false);
                    psStar2.Play(true);
                }
                else if (flowIndex == 3)
                {
                    ScaleShow(star3.transform, false);
                    psStar3.Play(true);
                }
            }
        }

        private void EndAddStar()
        {
            var crtLevelItem = LevelService.instance.runtimeLevel.levelItem;
            _nextAction = ShowPanelLoots;

            if (!crtLevelItem.passed || crtLevelItem.saveData.highScore < _score || crtLevelItem.saveData.highStar < _starCount)
            {
                Item cabReward = null;
                if (!crtLevelItem.passed)
                    LevelService.instance.FirstPassCrtLevel(ref cabReward);
                //Debug.Log("highscore " + crtLevelItem.saveData.highScore + " to " + _score);
                //Debug.Log("highStar " + crtLevelItem.saveData.highStar + " to " + _starCount);
                if (crtLevelItem.saveData.highStar < _starCount)
                    crtLevelItem.saveData.highStar = _starCount;
                if (crtLevelItem.saveData.highStar < _score)
                    crtLevelItem.saveData.highScore = _score;

                UxService.instance.SaveGameData();

                if (cabReward == null)
                {
                    ScaleShow(txtHighScore, true);
                }
                else
                {
                    ShowCabReward(cabReward);
                }
                return;
            }

            _nextAction?.Invoke();
        }

        void ShowCabReward(Item cabReward)
        {
            SoundService.instance.Play("reward");
            var protoReward = ItemService.instance.GetPrototype(cabReward.id);
            var proto = CombatAbilityService.instance.GetPrototype(protoReward.itemOutPut.id);
            WindowService.instance.ShowCombatAbilityPopup(proto, true, () => { ScaleShow(txtHighScore, true); });
        }

        private void ShowPanelLoots()
        {
            //Debug.Log("ShowPanelLoots");
            MainHudBehaviour.instance.Hide();
            if (_pureLoots.Count <= 0)
            {
                ShowButtons();
                return;
            }

            var h = dynamicPanelH_loots;
            //if (_pureLoots.Count <= 6)
            //{
            //    h = dynamicPanelH_loots * 0.65f;
            //}
            if (expMainText.transform.gameObject.activeSelf)
            {
                //move up expMainText
                expTexts.DOAnchorPosY(inPanelTextsAnchoredYAdd + expTexts.anchoredPosition.y, 0.35f, true);
            }
            if (scoreMainText.transform.gameObject.activeSelf)
            {
                //move up expMainText
                scoreTexts.DOAnchorPosY(inPanelTextsAnchoredYAdd + scoreTexts.anchoredPosition.y, 0.35f, true);
            }

            dynamicPanel.DOSizeDelta(new Vector2(dynamicPanelW_loots, h), 0.35f, true).OnComplete(
                () =>
                {
                    var percent = getGoldBonusPercent();
                    var gold = 0;
                    Item goldItem = null;
                    foreach (var loot in _pureLoots)
                    {
                        if (loot.id == "Gold")
                        {
                            goldItem = loot;
                            gold = loot.n;
                            break;
                        }
                    }
                    int bonusValue = MathGame.GetPercentage(gold, percent);
                    if (bonusValue > 0)
                    {
                        var descString = bonusValue + "(+" + percent + "%)";
                        lootsDescText.text = LocalizationService.instance.GetLocalizedTextFormatted("RDED_lootDesc", descString);
                        ScaleShow(lootsDescText.transform, false, 0.35f, 0.3f);
                        goldItem.n = bonusValue + gold;
                    }

                    _nextAction = ShowLootsItems;
                    lootsMainText.text = LocalizationService.instance.GetLocalizedText("RDED_lootMain");
                    ScaleShow(lootsMainText.transform, true);
                });
        }

        private void ShowLootsItems()
        {
            _nextAction = ShowButtons;
            if (_pureLoots.Count > 0)
            {
                var newList = new List<Item>();
                var cfg = ConfigService.instance.itemConfig;

                foreach (var c in cfg.list)
                {
                    foreach (var item in _pureLoots)
                    {
                        if (item != null && c != null && c.id == item.id)
                        {
                            if (item.n > 0)
                                newList.Add(new Item(item.n, item.id));
                            break;
                        }
                    }
                }

                wib.Setup(newList);
                foreach (var loot in _pureLoots)
                    UxService.instance.AddItem(loot);

                _nextAction?.Invoke();
                return;
            }

            _nextAction?.Invoke();
        }

        private void ShowButtons()
        {
            //Debug.Log("ShowButtons");
            UxService.instance.SaveGameData();
            UxService.instance.SaveGameItemData();
            var tweenSequence = DOTween.Sequence();
            tweenSequence.InsertCallback(0.35f, () =>
            {
                ScaleShow(quitButton.transform, false);
                if (hasNextLevel())
                    ScaleShow(nextLevelButton.transform, false);
            });
            tweenSequence.Play();
        }

        private int getExpBonusPercent()
        {
            return UxService.instance.GetItemAmount("FlagExp");
        }

        private int getGoldBonusPercent()
        {
            return UxService.instance.GetItemAmount("FlagGold");
        }

        private bool hasNextLevel()
        {
            if (!_winOrLoose)
                return false;
            //Debug.Log("hasNextLevel" + LevelService.instance.GetRuntimeCampaignLevelIndex() + " - - " + LevelService.instance.GetNextCampaignLevelIndex());

            var cli = LevelService.instance.GetRuntimeCampaignLevelIndex();
            if (cli < 0)
                return false;

            var nli = LevelService.instance.GetNextCampaignLevelIndex();
            if (cli < nli - 1)   //is re-play passed level
                return false;

            if (!LevelService.instance.HasNextCampaignLevel())
                return false;

            var levelId = LevelService.instance.GetCampainLevelId(cli);
            LevelPrototype crtLevel = LevelService.instance.GetPrototype(levelId);
            if (crtLevel.disableNextLevel)
                return false;

            return true;
        }

        public void OnClickNextLevel()
        {
            //Debug.Log("OnClickNextLevel");
            ResetState();
            OnClickBtnClose();

            var gameFlow = GameFlowService.instance;
            gameFlow.SetPausedState(GameFlowService.PausedState.Normal);
            LevelService.instance.SetupNextCompaignLevel();
            LevelService.instance.ClearLevel();

            LevelService.instance.StartLevel();
        }

        public void OnClickQuit()
        {
            //Debug.Log("OnClickQuit");
            ResetState();
            OnClickBtnClose();

            var gameFlow = GameFlowService.instance;
            CameraControllerBehaviour.instance.EnterPort();
            LevelService.instance.ClearLevel();
            gameFlow.SetPausedState(GameFlowService.PausedState.Normal);
            gameFlow.SetWindowState(GameFlowService.WindowState.Main);
        }
    }
}
