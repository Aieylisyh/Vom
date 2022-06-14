using UnityEngine;
using System.Collections.Generic;
using com;
namespace game
{
    public class CombatService : MonoBehaviour
    {
        public static CombatService instance { get; private set; }

        public float combatTime { get; private set; }
        public bool combatStopped { get; private set; }

        public PlayerShip playerShip;
        //   [HideInInspector]
        public List<Unit> units { get; private set; }

        //public int sessionAdCabExposeCount;
        //public int sessionAdCabUsedCount;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            combatStopped = true;
            //sessionAdCabExposeCount = 0;
            //sessionAdCabUsedCount = 0;
        }

        public void StartTimer()
        {
            combatTime = 0;
            combatStopped = false;
            Debug.Log("CombatService StartTimer!");
        }

        public void StopTimer()
        {
            combatStopped = true;
        }

        public void CombatUpdate()
        {
            combatTime += com.GameTime.deltaTime;
            LevelService.instance.CombatUpdate(combatTime);
        }

        private void Update()
        {
            if (combatStopped)
            {
                return;
            }

            CombatUpdate();
        }

        public void Register(Unit e)
        {
            if (units == null)
            {
                units = new List<Unit>();
            }
            units.Add(e);
        }

        //Clear all enemies and enemy projectiles except boss
        public void ClearLevelForBossSlay()
        {
            var newList = new List<Unit>();
            if (units != null)
            {
                foreach (var u in units)
                {
                    var needDestroy = false;
                    if (u is Enemy && !(u is Boss))
                    {
                        needDestroy = true;
                    }
                    else if (u is Projectile)
                    {
                        Projectile p = u as Projectile;
                        if (p.collision.selfFlag == UnitCollision.CollisionFlag.EnemyProjectile || p.collision.selfFlag == UnitCollision.CollisionFlag.Enemy)
                        {
                            needDestroy = true;
                        }
                    }

                    if (needDestroy)
                    {
                        u.Recycle();
                    }
                    else
                    {
                        newList.Add(u);
                    }
                }
            }
            units = newList;
        }

        public void ClearUnits()
        {
            if (units != null)
            {
                foreach (var u in units)
                {
                    u.Recycle();
                }
            }
            units = new List<Unit>();
        }

        public bool IsLevelCleared()
        {
            foreach (var u in units)
            {
                if (!u.IsAlive())
                {
                    continue;
                }

                if (u is Enemy)
                {
                    return false;
                }
                if (u is Loot)
                {
                    return false;
                }
                if (u is Projectile && u.collision.selfFlag == UnitCollision.CollisionFlag.EnemyProjectile)
                {
                    return false;
                }
            }

            return true;
        }

        public void LogUnits()
        {
            Debug.Log("LogUnits units num:" + units.Count);
            foreach (var u in units)
            {
                Debug.Log(u.gameObject);
            }
        }

        public void CleanDeadUnits()
        {
            units.RemoveAll(u => !u.IsAlive());
        }

        public string GetBackupShipId()
        {
            var res = "";
            return res;
        }

        public void OnCombatEnter()
        {
            playerShip.shipModelSwitcher.islandBehaviour.SetOutlineNone();
            playerShip.Reposition();

            WindowService.instance.OnCambatView();
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
        }

        public void OnCombatExit()
        {
            StartGameService.instance.OnReEnterPort();
            CreatePlayerAttri();
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
        }

        public PlayerAttributes playerAttri { get; private set; }

        public void RefreshPlayerAttri(PlayerAttributes.PlayerAttriLayer layer = PlayerAttributes.PlayerAttriLayer.Ship)
        {
            playerAttri.Refresh(PlayerAttributes.PlayerAttriLayer.Ship);
        }

        public void ResetPlayerAttri()
        {
            playerAttri.HardReset();
        }

        public void CreatePlayerAttri()
        {
            playerAttri = new PlayerAttributes(ShipService.instance.currentShipId);
            RefreshPlayerAttri();
            ResetPlayerAttri();
        }

        public void AoeDamageToProjectiles(Vector3 pos, float range, int maxHit)
        {
            List<Unit> targets = new List<Unit>();
            int restHit = maxHit;
            foreach (var u in units)
            {
                if (!u.IsAlive())
                {
                    continue;
                }

                bool isEneProjectile = u is Projectile && u.collision.selfFlag == UnitCollision.CollisionFlag.EnemyProjectile;
                if (isEneProjectile)
                {
                    var uPos = (u.move == null ? u.transform.position : u.move.transform.position);
                    var distVector = pos - uPos;
                    distVector.z = 0;
                    var dist = distVector.magnitude;
                    if (dist > range)
                    {
                        continue;
                    }
                    if (restHit > 0)
                    {
                        targets.Add(u);
                        restHit--;

                        if (restHit == 0)
                        {
                            break;
                        }
                    }

                }
            }

            foreach (var u in targets)
            {
                if (u.death != null)
                {
                    u.death.Die(false);
                }
                else
                {
                    u.Recycle();
                }
            }
        }

        public void AoeDamageToEnemy(bool affectEnemyProjectile, Damage dmg, float range, Vector3 pos, Unit exception = null)
        {
            List<Unit> targets = new List<Unit>();

            foreach (var u in units)
            {
                if (u == exception)
                {
                    continue;
                }
                if (!u.IsAlive())
                {
                    continue;
                }
                bool isEnemy = u is Enemy;
                bool isEneProjectile = u is Projectile && u.collision.selfFlag == UnitCollision.CollisionFlag.EnemyProjectile;
                if ((affectEnemyProjectile && isEneProjectile) || isEnemy)
                {
                    var uPos = (u.move == null ? u.transform.position : u.move.transform.position);
                    var distVector = pos - uPos;
                    distVector.z = 0;
                    var dist = distVector.magnitude;
                    //Debug.Log("Aoe ene dist " + dist + " -" + u.gameObject);
                    if (dist > range)
                    {
                        continue;
                    }

                    targets.Add(u);
                }
            }

            foreach (var u in targets)
            {
                if (u.health != null)
                {
                    u.health.OnReceiveDamage(dmg);
                }
                else if (u.death != null)
                {
                    u.death.Die(false);
                }
            }
        }
    }
}