using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class StatuesBehaviour : MonoBehaviour
    {
        public GameObject effect;
        public GameObject statue_Puck;
        public GameObject statue_Schierke;
        public GameObject statue_Griffith;

        public static StatuesBehaviour instance;

        public string correspondShipAbilityId1 = "Puck_ab_7";
        public string correspondShipAbilityId2 = "Schierke_ab_7";
        public string correspondShipAbilityId3 = "Griffith_ab_7";

        private void Start()
        {
            instance = this;
        }

        public void TryShowStatues()
        {
            var has1 = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId1);
            var has2 = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId2);
            var has3 = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId3);

            effect.SetActive(has1 || has2 || has3);
            statue_Puck.SetActive(has1);
            statue_Schierke.SetActive(has2);
            statue_Griffith.SetActive(has3);
        }
    }
}
