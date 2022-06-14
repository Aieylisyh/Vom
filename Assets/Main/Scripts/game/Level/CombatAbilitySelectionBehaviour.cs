using UnityEngine;
using System.Collections.Generic;
using com;
using DG.Tweening;

namespace game
{
    public class CombatAbilitySelectionBehaviour : PopupBehaviour
    {
        public static CombatAbilitySelectionBehaviour instance;
        private string _selectedId;
        private string _id1;
        private string _id2;
        private string _id3;

        public CombatAbilityChoiceBehaviour cabc1;
        public CombatAbilityChoiceBehaviour cabc2;
        public CombatAbilityChoiceBehaviour cabc3;

        private CombatAbilityChoiceBehaviour _selectedCabc;
        private CombatAbilityChoiceBehaviour[] _cabs;
        public GameObject selectButton;
        public float sizeBase = 1.0f;
        public float sizeSmall = 0.6f;
        public float sizeBig = 1.2f;

        private void Awake()
        {
            instance = this;
            _cabs = new CombatAbilityChoiceBehaviour[3];
            _cabs[0] = cabc1;
            _cabs[1] = cabc2;
            _cabs[2] = cabc3;
        }

        public void Setup(List<string > picked)
        {
            SoundService.instance.Play("tuto");
            _id1 = picked[0];
            _id2 = picked[1];
            _id3 = picked[2];

            float deltaT = 0.25f;
            cabc1.Show(_id1, 0);
            cabc2.Show(_id2, deltaT);
            cabc3.Show(_id3, deltaT * 2);
            selectButton.SetActive(false);

            var scale0 = Vector3.one * sizeBase;
            cabc1.transform.localScale = scale0;
            cabc2.transform.localScale = scale0;
            cabc3.transform.localScale = scale0;
        }

        public void OnClick1()
        {
            _selectedId = _id1;
            _selectedCabc = cabc1;
            OnSelect();
        }

        public void OnClick2()
        {
            _selectedId = _id2;
            _selectedCabc = cabc2;
            OnSelect();
        }

        public void OnClick3()
        {
            _selectedId = _id3;
            _selectedCabc = cabc3;
            OnSelect();
        }

        private void OnSelect()
        {
            foreach (var c in _cabs)
            {
                if (c != _selectedCabc)
                {
                    c.transform.DOKill();
                    c.transform.DOScale(sizeSmall, 0.35f);
                    c.HideInfo();
                }
            }
            _selectedCabc.transform.DOKill();
            _selectedCabc.transform.DOScale(sizeBig, 0.35f).SetEase(Ease.OutCubic);
            _selectedCabc.ShowInfo();

            SoundService.instance.Play("btn big");
            selectButton.SetActive(true);
        }

        public void OnSelectConfirmed()
        {
            CombatAbilityService.instance.AddToSelectedPool(_selectedId);
            SoundService.instance.Play("choose cab");

            foreach (var c in _cabs)
            {
                if (c != _selectedCabc)
                {
                    c.SelectHide();
                }
            }

            if (_selectedId == "ca_Reboot")
            {
                var picked = CombatAbilityService.instance.Get3ToSelect(3);
                _selectedCabc.SelectedFeedback(() => { Setup(picked); });
            }
            else
            {
                _selectedCabc.SelectedFeedback(() => { OnSelectEnd(); });
            }

            selectButton.SetActive(false);
        }

        public void OnSelectEnd()
        {
            //var title = LocalizationService.instance.GetLocalizedText(_selectedCabc.proto.title);
            //string titleString = "<size=120%><color=#EEE0C0>" + title + "</color></size>";
            var desc = LocalizationService.instance.GetLocalizedText(_selectedCabc.proto.desc);
            if (_selectedCabc.proto.hasIntValue)
                desc = LocalizationService.instance.GetLocalizedTextFormatted(_selectedCabc.proto.desc, _selectedCabc.proto.intValue);

            //string descString = "<size=100%><color=#FFFFFF>" + desc + "</color></size>";
            // FloatingTextPanelBehaviour.instance.Create(titleString + "\n\n" + descString, 0.5f, 0.45f, true);
            string descString = "<size=110%><color=#EEE0C0>" + desc + "</color></size>";
            FloatingTextPanelBehaviour.instance.Create(descString, 0.5f, 0.52f, true);
            LevelService.instance.EndCombatAbilitySelection();

            //effect
            CombatAbilityService.instance.ValidateSelected(_selectedId);

            var ph = CombatService.instance.playerShip.health as PlayerHealth;
            switch (_selectedCabc.proto.fxType)
            {
                case CombatAbilityPrototype.FxType.Buff:
                    ph.PlayBuffFx();
                    break;

                case CombatAbilityPrototype.FxType.Heal:
                    ph.PlayHealFx();
                    break;

                case CombatAbilityPrototype.FxType.Block:
                    ph.PlayBlockFx();
                    break;
            }
        }
    }
}