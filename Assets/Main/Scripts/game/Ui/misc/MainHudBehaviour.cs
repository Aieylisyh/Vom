using UnityEngine;
using UnityEngine.UI;
using com;
using DG.Tweening;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MainHudBehaviour : MonoBehaviour
    {
        public Text amountExp;
        public Text amountExpMax;
        public Text amountLevel;

        public Text amount1;
        public Text amount2;
        public Text amount3;
        public Text amount4;

        public HealthBar bar;
        public CanvasGroup cg;
        public GameObject playerLevelView;

        public static MainHudBehaviour instance { get; private set; }

        private bool _showPlayerLevel;
        private string _itemId1;
        private string _itemId2;
        private string _itemId3;
        private string _itemId4;

        public enum MainHudMode
        {
            Custom,
            ExpGoldDiamond,
            Tokens,
        }

        private void Awake()
        {
            instance = this;
        }

        public void SetMode(bool showPlayerLevel, string id1 = "", string id2 = "", string id3 = "", string id4 = "")
        {
            _showPlayerLevel = showPlayerLevel;
            _itemId1 = id1;
            _itemId2 = id2;
            _itemId3 = id3;
            _itemId4 = id4;

            Refresh();
        }

        public void RefreshToDefault()
        {
            SetMode(MainHudMode.ExpGoldDiamond);
            Show();
        }

        public void SetMode(MainHudMode mode)
        {
            if (mode == MainHudMode.Custom)
            {
                SetMode(false, "", "", "", "");
                return;
            }
            if (mode == MainHudMode.ExpGoldDiamond)
            {
                SetMode(true, "", "", "Gold", "Diamond");
                return;
            }
            if (mode == MainHudMode.Tokens)
            {
                SetMode(false, "Token1", "Token2", "Token3", "Token4");
                return;
            }
        }

        public void Refresh()
        {
            if (_showPlayerLevel)
            {
                playerLevelView.SetActive(true);
                SetExp();
            }
            else
            {
                playerLevelView.SetActive(false);
            }

            SetItem(amount1, _itemId1);
            SetItem(amount2, _itemId2);
            SetItem(amount3, _itemId3);
            SetItem(amount4, _itemId4);
        }

        private void SetExp(bool withAnim = false)
        {
            var data = UxService.instance.gameDataCache.cache;
            amountLevel.text = data.playerLevel + "";
            var crt = data.exp;
            var max = 10000;

            amountExpMax.text = "/" + max;
            if (withAnim)
            {
                bar.Set((float)crt / max, false);
                amountExp.DOText(crt + "", 0.5f, false);
                return;
            }

            bar.Set((float)crt / max, true);
            amountExp.text = crt + "";
        }

        private void SetItem(Text txt, string id, bool biggerIcon = true)
        {
            if (id == "")
            {
                txt.text = "";
                return;
            }

            var item = ItemService.instance.GetPrototype(id);
            var amount = UxService.instance.GetItemAmount(id);
            string amountString = TextFormat.GetBigNumberScientific(amount);
            if (biggerIcon)
            {
                txt.text = "<size=44>" + TextFormat.GetRichTextTag(id) + "</size> " + amountString;
            }
            else
            {
                txt.text = TextFormat.GetRichTextTag(id) + " " + amountString;
            }
        }

        public void Show()
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }

        public void Hide()
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }
}
