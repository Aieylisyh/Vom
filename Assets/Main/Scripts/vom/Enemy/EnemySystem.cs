using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EnemySystem : MonoBehaviour
    {
        public static EnemySystem instance { get; private set; }

        public List<EnemyBehaviour> enemies { get; private set; }
        public List<PlayerBehaviour> players { get; private set; }

        private void Awake()
        {
            instance = this;
            enemies = new List<EnemyBehaviour>();
            players = new List<PlayerBehaviour>();
        }

        private void Start()
        {
            players.Add(PlayerBehaviour.instance);
        }

        public void AddEnemy(EnemyBehaviour e)
        {
            enemies.Add(e);
        }

        public void RemoveEnemy(EnemyBehaviour e)
        {
            enemies.Remove(e);
        }

        public bool HasEnemyTargetedPlayer()
        {
            foreach (var e in enemies)
            {
                if (e.gameObject.activeSelf && !e.death.dead && e.targetSearcher.alerted)
                    return true;
            }

            return false;
        }

        public PlayerBehaviour[] GetValidPlayers()
        {
            //TODO
            if (players[0].health.dead)
                return null;
            var p = new PlayerBehaviour[] { players[0] };

            return p;
        }
    }
}