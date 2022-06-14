using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using DG.Tweening;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MapPanelBehaviour : Ticker
    {
        public MapNode prefabMapNode;
        public MapNodePath prefabMapNodePath;

        public float distancePath = 30;
        public int ignoreStartCount = 1;
        public int ignoreEndCount = 1;

        public RectTransform parentRectTrans;
        public RectTransform parentPathRectTrans;
        public List<MapNode> nodes { get; private set; }

        public RectTransform contentRect;
        public float startContentY = 243;
        public float endContentY = -243;
        public float scrollContentDuration = 0.7f;

        public Text starsCount;
        public Slider idleProgressBar;

        public GameObject idleRewardNode;

        public GameObject bossNode;

        public GameObject abysseNode;

        public GameObject wikiNode;

        public List<GameObject> psDefault;

        public void RefreshMap()
        {
            //Debug.Log("RefreshMap");
            bool firstNotPassed = false;
            int levelPassIndex = LevelService.instance.GetNextCampaignLevelIndex();
            //Debug.Log("levelPassIndex " + levelPassIndex);
            foreach (var mnp in nodes)
            {
                var levelItem = LevelService.instance.GetLevelItem(mnp.proto.levelId);
                var levelProto = LevelService.instance.GetPrototype(mnp.proto.levelId);
                // int levelCampaignIndex = LevelService.instance.GetCampaignLevelIndex(mnp.proto.levelId);
                mnp.SetDisplayed(levelPassIndex >= levelProto.mapViewData.displayMinPassedLevelIndex);
                mnp.SetStars(levelItem.saveData.highStar);

                var passed = levelItem.passed;
                mnp.SetPassed(passed);

                if (levelProto.levelType == LevelPrototype.LevelType.Campaign)
                {
                    mnp.SetUnlocked(true);
                    if (!firstNotPassed && !levelItem.passed)
                    {
                        firstNotPassed = true;
                        mnp.SetShake(true);
                    }
                    else
                    {
                        mnp.SetShake(false);
                    }
                }
                else if (levelProto.levelType == LevelPrototype.LevelType.Extra)
                {
                    mnp.SetShake(false);
                    var unlocked = levelItem.saveData.unlocked;
                    mnp.SetUnlocked(unlocked);
                }
            }

            SetStarsCount();

            var cfg = ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData;
            wikiNode.SetActive(levelPassIndex >= cfg.wiki);
            idleRewardNode.SetActive(levelPassIndex >= cfg.mapIdle);
            bossNode.SetActive(levelPassIndex >= cfg.boss);
            abysseNode.SetActive(levelPassIndex >= cfg.abysse);
        }

        public void GenerateMap()
        {
            nodes = new List<MapNode>();

            //add campaignLevels
            var campaignLevels = ConfigService.instance.levelConfig.campaignLevels;
            foreach (var level in campaignLevels)
                nodes.Add(CreateMapNode(level));

            CreateCampaignLevelsPaths();

            //add extra levels
            var otherLevels = ConfigService.instance.levelConfig.otherLevels;
            foreach (var level in otherLevels)
            {
                if (level.levelType == LevelPrototype.LevelType.Extra)
                    nodes.Add(CreateMapNode(level));
            }
        }

        void SetStarsCount()
        {
            starsCount.text = LevelService.instance.GetStarCount() + "";
        }

        void CreateCampaignLevelsPaths()
        {
            var count = nodes.Count;
            for (var i = 0; i < count; i++)
            {
                var hasNext = i < count - 1;
                if (hasNext)
                {
                    var crtMapNode = nodes[i];
                    var nextMapNode = nodes[i + 1];
                    Vector2 pos = new Vector2(crtMapNode.proto.offsetX, crtMapNode.proto.offsetY);
                    Vector2 nextPos = new Vector2(nextMapNode.proto.offsetX, nextMapNode.proto.offsetY);
                    var distance = (nextPos - pos).magnitude;
                    int countPath = Mathf.FloorToInt(distance / distancePath);
                    crtMapNode.paths = new List<MapNodePath>();

                    for (var j = 0; j < countPath; j++)
                    {
                        if (j < ignoreStartCount || j > countPath - 1 - ignoreEndCount)
                            continue;

                        MapNodePath pathNode = Instantiate(prefabMapNodePath, parentPathRectTrans);
                        float indexFactor = (float)j / countPath;
                        pathNode.Setup(pos, nextPos, indexFactor, crtMapNode.proto.curveFactor);
                        crtMapNode.paths.Add(pathNode);
                    }
                }
            }
        }

        MapNode CreateMapNode(LevelPrototype level)
        {
            var levelItem = LevelService.instance.GetLevelItem(level.id);
            MapNodePrototype mnp = new MapNodePrototype();
            var mvd = level.mapViewData;
            mnp.offsetX = mvd.offsetX;
            mnp.offsetY = mvd.offsetY;
            mnp.curveFactor = mvd.curveFactor;
            mnp.nodeViewType = mvd.nodeViewType;
            mnp.passed = levelItem.passed;
            mnp.levelId = level.id;

            var mapNode = Instantiate<MapNode>(prefabMapNode, parentRectTrans);
            mapNode.gameObject.name = level.id;
            mapNode.Setup(mnp);
            return mapNode;
        }

        public void ScrollMap()
        {
            contentRect.DOKill();
            contentRect.anchoredPosition = new Vector2(0, startContentY);
            contentRect.DOAnchorPosY(endContentY, scrollContentDuration).SetEase(Ease.InOutCubic);
        }

        void ClearMap()
        {
            var count = parentRectTrans.childCount;
            for (var i = count - 1; i > -1; i--)
            {
                var go = parentRectTrans.GetChild(i);
                if (go != prefabMapNode.gameObject && go != prefabMapNodePath.gameObject)
                {
                    Destroy(go.gameObject);
                }
            }
        }

        public void HideParticles()
        {
            foreach (var ps in psDefault)
                ps.SetActive(false);
        }

        public void ShowParticles()
        {
            foreach (var ps in psDefault)
                ps.SetActive(true);
        }

        public void OnClickIdle()
        {
            LevelIdleService.instance.TryClaimReward();
        }

        void UpdateIdleState()
        {
            if (!idleRewardNode.activeSelf)
                return;

            var percent = LevelIdleService.instance.GetIdleTimePercent();
            idleProgressBar.value = percent;
            if (percent == 1)
            {
                //TODO effect
            }
        }

        protected override void Tick()
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main)
            {
                UpdateIdleState();
            }
        }

        public void ForceTick()
        {
            Tick();
        }

        public void ClickBossButton()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("BossPendingTitle");
            data.content = LocalizationService.instance.GetLocalizedText("BossPendingContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickDungeonButton()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("DungeonPendingTitle");
            data.content = LocalizationService.instance.GetLocalizedText("DungeonPendingContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickWikiButton()
        {
            LevelService.instance.EnterPediaLevel();
        }
    }
}