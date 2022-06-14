using UnityEngine;
using DG.Tweening;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MapNodeInfoPanelBehaviour : MonoBehaviour
    {
        public static MapNodeInfoPanelBehaviour instance;

        public WindowTabsBehaviour wtb;
        public MapNodePlayTabBehaviour mapNodePlayTabBehaviour;
        public MapNodeRaidTabBehaviour mapNodeRaidTabBehaviour;

        public MapNodeInfoPanelStarsBehaviour stars;//done

        public ParticleSystem psRaidDone;//done
        public ParticleSystem psUnlockDone;//done

        public Text levelTitleTxt;//done
        public Text levelTypeTxt;//done

        public Text levelDescTxt;//done
        public Text levelWaveInfoTxt;//done
        public Text levelBossInfoTxt;//done

        public string levelId { get; private set; }//done

        public float startHeightReel;//done
        public float endHeightReel;//done
        public float widthReel;//done
        public float animeDuration;//done

        public Image reel;//done
        public GameObject view;//done

        public RectTransform tabPanel;

        private void Awake()
        {
            instance = this;
        }

        public LevelItem levelItem { get { return LevelService.instance.GetLevelItem(levelId); } }
        public LevelPrototype levelProto { get { return LevelService.instance.GetPrototype(levelId); } }

        public void Setup(string pId)
        {
            levelId = pId;
            PlayAnimation();
        }

        void PlayAnimation()
        {
            view.SetActive(false);

            var proto = levelProto;
            var item = levelItem;
            levelTitleTxt.text = LocalizationService.instance.GetLocalizedText(proto.title);

            var levelTypeString = LocalizationService.instance.GetLocalizedText("MNI_" + proto.levelType.ToString());
            if (proto.levelType == LevelPrototype.LevelType.Campaign)
                levelTypeString += " " + (LevelService.instance.GetCampaignLevelIndex(proto) + 1);
            levelTypeTxt.text = levelTypeString;

            stars.Hide();
            HideTabs();

            reel.rectTransform.DOKill();
            reel.rectTransform.sizeDelta = new Vector2(widthReel, startHeightReel);
            reel.rectTransform.DOSizeDelta(new Vector2(widthReel, endHeightReel), animeDuration).SetEase(Ease.OutBack).OnComplete(OnCompleteAnim);
            SoundService.instance.Play("scroll open");
        }

        void OnCompleteAnim()
        {
            view.SetActive(true);
            var proto = levelProto;
            var item = levelItem;

            ShowStars(item);
            levelDescTxt.text = LocalizationService.instance.GetLocalizedText(proto.desc);

            var waves = proto.waveCount;
            if (waves == 0)
            {
                levelWaveInfoTxt.text = "";
            }
            else
            {
                levelWaveInfoTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("MNI_Wave", waves);
            }

            var hasBoss = (proto.mapViewData.nodeViewType != MapNodePrototype.NodeViewType.Normal);
            levelBossInfoTxt.text = LocalizationService.instance.GetLocalizedText(hasBoss ? "MNI_HasBoss" : "MNI_NoBoss");

            var passed = item.passed;
            var indexNextCampaign = LevelService.instance.GetNextCampaignLevelIndex();
            var indexCrtCampaign = LevelService.instance.GetCampaignLevelIndex(proto);
            //Debug.Log(indexNextCampaign);
            //Debug.Log(indexCrtCampaign);
            var reached = indexCrtCampaign <= indexNextCampaign;

            if (passed)
            {
                ShowTabs();
                var showRaid = IsTutoEnableRaid();
                if (showRaid)
                {
                    wtb.SetTo2TabsLayout(0, 1);
                    ShowRaidTab();
                }
                else
                {
                    wtb.SetTo1TabLayout(0);
                    ShowPlayTab();
                }
            }
            else if (reached)
            {
                ShowTabs();
                wtb.HideAll();
                //wtb.SetTo1TabLayout(0);
                ShowPlayTab();
            }
        }

        void ShowStars(LevelItem item)
        {
            var passed = item.passed;
            var starCount = item.saveData.highStar;
            stars.ShowAndClear();
            if (passed)
            {
                if (starCount == 1)
                {
                    stars.Show1Star();
                }
                else if (starCount == 2)
                {
                    stars.Show2Star();
                }
                else if (starCount == 3)
                {
                    stars.Show3Star();
                }
            }
        }

        public void OnRaidDone()
        {
            SoundService.instance.Play(new string[2] { "pay1", "pay2" });
            psRaidDone.Play(true);
        }

        public void OnUnlockDone()
        {
            SoundService.instance.Play(new string[2] { "pay1", "pay2" });
            psUnlockDone.Play(true);
        }

        void HideTabs()
        {
            tabPanel.gameObject.SetActive(false);
        }

        void ShowTabs()
        {
            tabPanel.DOKill();
            tabPanel.localScale = Vector3.one * (0.5f);
            tabPanel.gameObject.SetActive(true);
            tabPanel.DOScale(1, 0.35f).SetEase(Ease.OutCubic);
            SoundService.instance.Play("btn big");
        }

        public void ShowPlayTab()
        {
            wtb.SetTab(0);
            OnShowPlayTab();
        }

        public void ShowRaidTab()
        {
            wtb.SetTab(1);
            OnShowRaidTab();
        }

        private void OnShowPlayTab()
        {
            mapNodePlayTabBehaviour.Refresh();
        }

        private void OnShowRaidTab()
        {
            mapNodeRaidTabBehaviour.Refresh();
        }

        public void OnClickQuit()
        {
            MapWindowBehaviour.instance.ShowMap();
        }

        bool IsTutoEnableRaid()
        {
            var li = LevelService.instance.GetNextCampaignLevelIndex();
            var cfg = ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData;

            return li >= cfg.raid;
        }
    }
}