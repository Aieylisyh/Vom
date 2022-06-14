using UnityEngine;
using System.Collections;

namespace game
{
    public class PlayerTorpedoModelSetup : MonoBehaviour
    {
        public GameObject magicTor;
        public GameObject standardTor;
        public GameObject bioTor;

        public string bioMatchShipId = "Griffith";
        public string bioMatchSabId = "Griffith_ab_1";

        public string magicMatchShipId = "Schierke";
        public string magicMatchSabId = "Schierke_ab_1";
        public string overrideDieEffectIdMagic = "exp magic";
        public string overrideDieEffectIdBio = "exp projectile";
        public void Set(Torpedo tor)
        {
            ProjectileDeath pd = tor.death as ProjectileDeath;
            pd.overrideDieEffectId = "";

            if (ShipService.instance.currentShipId == bioMatchShipId)
            {
                if (ShipService.instance.HasAnyShipUnlockedAbility(bioMatchSabId))
                {
                    pd.overrideDieEffectId = overrideDieEffectIdBio;
                    SetBio(tor);
                    return;
                }
            }

            if (ShipService.instance.currentShipId == magicMatchShipId)
            {
                if (ShipService.instance.HasAnyShipUnlockedAbility(magicMatchSabId))
                {
                    pd.overrideDieEffectId = overrideDieEffectIdMagic;
                    SetMagic(tor);
                    return;
                }
            }

            SetStandard(tor);
        }

        private void SetMagic(Torpedo tor)
        {
            magicTor.SetActive(true);
            standardTor.SetActive(false);
            bioTor.SetActive(false);
            tor.hasFire = false;
            tor.hasBubble = false;
        }
        private void SetBio(Torpedo tor)
        {
            magicTor.SetActive(false);
            standardTor.SetActive(false);
            bioTor.SetActive(true);
            tor.hasFire = false;
            tor.hasBubble = true;
        }
        private void SetStandard(Torpedo tor)
        {
            magicTor.SetActive(false);
            standardTor.SetActive(true);
            bioTor.SetActive(false);
            tor.hasFire = true;
            tor.hasBubble = true;
        }
    }

}
