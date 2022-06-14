using UnityEngine;
using System.Collections;

namespace game
{
    public class PlayerBombModelSetup : MonoBehaviour
    {
        public GameObject hugeBumb;//TODO
        public GameObject standardBomb;
        public GameObject bioBomb;
        public GameObject anchorBomb;

        public string bioMatchShipId = "Griffith";
        public string bioMatchSabId = "Griffith_ab_1";

        public string anchorMatchShipId = "Puck";
        public string anchorMatchSabId = "Puck_ab_1";
        public string overrideDieEffectIdAnchor = "exp anchor";
        public void Set(Bomb bomb)
        {
            ProjectileDeath pd = bomb.death as ProjectileDeath;
            pd.overrideDieEffectId = "";

            if (ShipService.instance.currentShipId == bioMatchShipId)
            {
                if (ShipService.instance.HasAnyShipUnlockedAbility(bioMatchSabId))
                {
                    //pd.overrideDieEffectId = overrideDieEffectIdBio;
                    SetBio(bomb);
                    return;
                }
            }

            if (ShipService.instance.currentShipId == anchorMatchShipId)
            {
                if (ShipService.instance.HasAnyShipUnlockedAbility(anchorMatchSabId))
                {
                    pd.overrideDieEffectId = overrideDieEffectIdAnchor;
                    SetAnchor(bomb);
                    return;
                }
            }

            SetStandard(bomb);
        }

        private void SetAnchor(Bomb bomb)
        {
            anchorBomb.SetActive(true);
            standardBomb.SetActive(false);
            bioBomb.SetActive(false);
            bomb.hasFire = false;
            bomb.hasBubble = true;

        }
        private void SetBio(Bomb bomb)
        {
            anchorBomb.SetActive(false);
            standardBomb.SetActive(false);
            bioBomb.SetActive(true);
            bomb.hasFire = false;
            bomb.hasBubble = true;
        }
        private void SetStandard(Bomb bomb)
        {
            anchorBomb.SetActive(false);
            standardBomb.SetActive(true);
            bioBomb.SetActive(false);
            bomb.hasFire = false;
            bomb.hasBubble = true;
        }
    }

}
