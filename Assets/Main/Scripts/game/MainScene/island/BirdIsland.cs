using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class BirdIsland : MonoBehaviour
    {
        public GameObject whiteBirds;

        public static BirdIsland instance;

        public string correspondShipAbilityId = "Griffith_ab_2";

        private void Start()
        {
            instance = this;
        }

        public void TryShowBird()
        {
            //Debug.Log("TryShowBird");
            var has = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId);

            if (has && !whiteBirds.activeSelf)
            {
                //Debug.Log("show ");
                whiteBirds.SetActive(true);
            }
            else if (!has && whiteBirds.activeSelf)
            {
                //Debug.Log("hide ");
                whiteBirds.SetActive(false);
            }

        }
    }
}
