using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class PortalIsland : IslandBehaviour
    {
        public GameObject portal;
        public static PortalIsland instance;
        public string correspondShipAbilityId = "Schierke_ab_2";
        public ParticleSystem ps;
        private int _count;
        public int softGap = 666;

        private void Start()
        {
            instance = this;
            _count = 0;
        }

        public void TryShowPortal()
        {
            //Debug.Log("TryShowPortal");
            var has = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId);
            //Debug.Log("has " + has);
            portal.SetActive(has);
        }

        public override void ClickFunction()
        {
            var picks = ConfigService.instance.itemConfig.getComplexItem("portal").list;
            int r = Random.Range(0, picks.Count);
            var item = picks[r];

            _count++;
            if (_count > softGap)
            {
                item.n = 0;
            }
            else if (_count == 1)
            {
                item = new Item(10, "Gold");
            }
            else if(_count == softGap)
            {
                item = new Item(1, "Diamond");
            }

            Feedback(item);
        }

        void Feedback(Item item)
        {
            List<Item> items = new List<Item>();
            items.Add(item);
            ItemService.instance.GiveReward(items, true);

            ps.Play(true);
            SoundService.instance.Play("reward");
            FloatingTextPanelBehaviour.instance.Create(TextFormat.GetItemText(item, true), Random.Range(0.48f, 0.52f), Random.Range(0.82f, 0.85f));
            MainHudBehaviour.instance.Refresh();
        }
    }
}
