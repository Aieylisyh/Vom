using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;
using DG.Tweening;
using System.Collections.Generic;

namespace game
{
    public class PediaPanelPopup : PopupBehaviour
    {
        public Text titleText;
        public Text descText;
        public Text tierText;
        public Text indexText;
        public WindowInventoryBehaviour inv;
        public Slider hpSlider;
        public Slider attackSlider;

        private int _index = 0;

        public void Setup()
        {
            SetView();
        }

        public void SetToEnemy(string id)
        {
            SetToEnemy(PediaService.instance.GetPrototype(id));
        }

        public void SetToEnemy(int index)
        {
            _index = index;
            SetView();
        }

        public void SetToEnemy(PediaPrototype pedia)
        {
            var cfg = ConfigService.instance.pediaConfig;
            _index = cfg.pedias.IndexOf(pedia);
            titleText.text = LocalizationService.instance.GetLocalizedText(pedia.eneProto.title);
            descText.text = LocalizationService.instance.GetLocalizedText(pedia.eneProto.desc);

            bool isLight = pedia.eneProto.Category.tier == EnemyPrototype.EnemyCategory.Tier.Light;
            tierText.text = LocalizationService.instance.GetLocalizedText(isLight ? "PediaLight" : "PediaHeavy");
            //attackSlider.value = pedia.attackRate;
            //hpSlider.value = pedia.hpRate;
            hpSlider.DOValue(pedia.hpRate, duration).SetEase(Ease.OutCubic);
            attackSlider.DOValue(pedia.attackRate, duration).SetEase(Ease.OutCubic);

            var items = new List<Item>();
            foreach (var dropItem in pedia.eneProto.dropData.dropItems)
            {
                var drop = dropItem.item;
                if (string.IsNullOrEmpty(drop.id) || drop.n <= 0)
                    continue;

                if (drop.id=="tokens")
                    continue;

                var complex = ConfigService.instance.itemConfig.getComplexItem(drop.id);
                if (complex!=null)
                {
                    foreach (var com in complex.list)
                    {
                        //Debug.Log(com.id);
                        items.Add(new Item(0, com.id));
                    }
                    continue;
                }

                items.Add(new Item(0, drop.id));
            }

            inv.Setup(items);
            LevelHudBehaviour.instance.Hide();
            indexText.text = (_index + 1) + "/" + cfg.pedias.Count;

            PediaService.instance.SetView(pedia.eneProto);
        }

        public void OnClickNext()
        {
            _index++;
            SetView();
        }

        public void OnClickLast()
        {
            _index--;
            SetView();
        }

        public void OnClickTip()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("PediaTitle");
            data.content = LocalizationService.instance.GetLocalizedText("PediaContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickExit()
        {
            OnClickBtnClose();
            PediaService.instance.ExitPedia();
        }

        void SetView()
        {
            var cfg = ConfigService.instance.pediaConfig;
            var count = cfg.pedias.Count;
            if (_index >= count)
                _index = 0;
            if (_index < 0)
                _index = count - 1;

            SetToEnemy(cfg.pedias[_index]);
        }
    }
}