using System;
using com;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class BossHealth : EnemyHealth
    {
        public float hpRatioToUseUltra = 0.5f;

        public BossSkillPrototype ultra;

        private bool _usedUltra;

        public bool lureLaunchBossTor;
        private bool _receivedBossCinematicTor;
        private bool _luredBossCinematicTor;

        public override void ResetState()
        {
            base.ResetState();
            _usedUltra = false;
            _receivedBossCinematicTor = false;
            _luredBossCinematicTor = false;
        }

        protected override bool TryDie()
        {
            //UnityEngine.Debug.Log("TryDie" + hp);
            if (hp <= 0)
            {
                //UnityEngine.Debug.Log("hp<0");
                if (CanTriggerBossKill())
                {
                    //UnityEngine.Debug.Log("CanTriggerBossKill no dmg");
                    TriggerBossKill();
                    if (hp < 1)
                        hp = 1;
                    hb.Hide();
                    return false;
                }
                //UnityEngine.Debug.Log("die");
                ClearHealth();
                self.death.Die(false);
                return true;
            }

            if (hp < 1)
                hp = 1;

            return false;
        }

        bool CanTriggerBossKill()
        {
            if (CombatService.instance.playerShip.death.isDead)
                return false;
            //UnityEngine.Debug.Log("CanTriggerBossKill 1");
            if (_receivedBossCinematicTor)
                return false;
            //UnityEngine.Debug.Log("CanTriggerBossKill 2");
            return true;
        }

        void TriggerBossKill()
        {
            if (_luredBossCinematicTor)
                return;
            //UnityEngine.Debug.Log("TriggerBossKill");
            hb.Hide();
            CombatService.instance.playerShip.playerAttack.LaunchBossCinematicTors();
            _luredBossCinematicTor = true;
            CombatService.instance.ClearLevelForBossSlay();
        }

        protected override void SetHealthBar(float r)
        {
            if (_luredBossCinematicTor)
            {
                hb.Hide();
                return;
            }

            hb?.Set(r);
        }

        public override void OnReceiveDamage(Damage damage = null)
        {
            if (damage == null)
                return;
            //UnityEngine.Debug.Log(gameObject + "OnReceiveDamage " + damage.value);
            if (damage.isBossKiller)
            {
                //UnityEngine.Debug.Log("isBossKiller _receivedBossCinematicTor");
                _receivedBossCinematicTor = true;
                CameraControllerBehaviour.instance.SetSwitchBackTimer(2);
                hp = -1;
                CombatService.instance.ClearLevelForBossSlay();
            }

            if (GetIsInvinsible())
            {
                TriggerInvinsible();
                //UnityEngine.Debug.Log("TriggerInvinsible ");
                return;
            }

            //UnityEngine.Debug.Log("OnReceiveDamage  value " + damage.value);
            var delta = RefineDamageValue(damage);
            //UnityEngine.Debug.Log("delta " + delta);
            OnHealthChange(-delta);
            postDamaged(damage, -delta);
        }

        protected override void Tick()
        {
            base.Tick();

            if (!_usedUltra && HealthRatio < hpRatioToUseUltra)
            {
                TryUseUltra();
            }
        }

        void TryUseUltra()
        {
            var bossAi = (self as Boss).bossAi;
            bossAi.SetQueuedSkill(ultra);
            _usedUltra = true;
        }
    }
}
