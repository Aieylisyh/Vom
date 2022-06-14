using UnityEngine;
using com;

namespace game
{
    public class PlayerHealth : UnitHealth
    {
        public ShakeBehaviour camShakeBehaviour;
        private float _invinsibleTimer;

        public ParticleSystem invinsiblePs;//need toggle off
        public ParticleSystem blockPs;
        public ParticleSystem healPs;//need toggle off
        public ParticleSystem healOncePs;
        public ParticleSystem holylightPs;//need toggle off
        public ParticleSystem[] toggleOffPs;
        public ScreenVignetteBehaviour svb;

        private bool _lowHealthProtected;

        public override void ResetState()
        {
            _lowHealthProtected = false;
            svb.ChangeValue(0);

            foreach (var ps in toggleOffPs)
                ps.Stop(true);

            base.ResetState();
        }

        public override void RegTick()
        {
            base.RegTick();
        }

        protected override void Tick()
        {
            base.Tick();
            RegTick();
            if (_invinsibleTimer > 0)
            {
                _invinsibleTimer -= TickTime;
                if (_invinsibleTimer <= 0)
                {
                    invinsiblePs.Stop(true);
                }
            }
        }

        protected override float GetRegPerTick()
        {
            if (HealthRatio < 0.5f)
            {
                var attri = CombatService.instance.playerAttri;
                var resReg = MathGame.GetPercentageAdded(reg, attri.ttModifier.regAdd_below50p);
                //Debug.Log("GetReg " + resReg);
                return resReg * TickTime;
            }

            return reg * TickTime;
        }

        protected override bool GetIsInvinsible()
        {
            if (_invinsibleTimer > 0)
            {
                return true;
            }

            if (CameraControllerBehaviour.instance.cinematicCam.gameObject.activeSelf)
            {
                return true;
            }

            return false;
        }

        protected override void TriggerInvinsible()
        {
            PlayBlockFx();
        }

        protected override bool TryDie()
        {
            var cfg = ConfigService.instance.combatConfig.playerParam;
            if (hp <= 0 && !_lowHealthProtected)
            {
                if (hp > cfg.lowHealthProtectMinValue)
                {
                    _lowHealthProtected = true;
                    hp = 1;
                }
            }

            if (hp <= 0)
            {
                var attri = CombatService.instance.playerAttri;
                var spareEngineTime = attri.ttModifier.spareEngineSec;
                //Debug.Log("TryDie spareEngineTime " + spareEngineTime);
                //Debug.Log(LevelService.instance.runtimeLevel.GetSpareEngineTriggerCount());
                if (spareEngineTime > 0 && LevelService.instance.runtimeLevel.GetSpareEngineTriggerCount() == 0)
                {
                    TriggerSpareEngine();
                    return false;
                }

                ClearHealth();
                self.death.Die(false);
                return true;
            }

            if (hp < 1)
                hp = 1;

            return false;
        }

        private void TriggerSpareEngine()
        {
            LevelService.instance.runtimeLevel.AddSpareEngineTriggerCount();
            var attri = CombatService.instance.playerAttri;
            var spareEngineTime = attri.ttModifier.spareEngineSec;
            var spareEngineRestore = (float)attri.ttModifier.hpRestoreSpareEngine / 100f * hpMax + 1;
            OnHealthChange((int)spareEngineRestore);
            _invinsibleTimer = spareEngineTime;
            invinsiblePs.Play(true);
            blockPs.Play(true);
            SoundService.instance.Play("invincible");
            //Debug.Log("TriggerSpareEngine TODO Effect sound spareEngineRestore " + spareEngineRestore);
        }

        protected override void OnHealthChange(int v)
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main)
                return;

            hp = Mathf.Clamp(hp + v, 0, hpMax);
            //Debug.Log(self.gameObject + " hpChange v" + v + " hp " + hp + "/" + hpMax);
            if (TryDie())
                return;

            SetHealth(hp);
            //Debug.Log("player HealthChange " + v);
            if (v < 0)
            {
                //Debug.Log(HealthRatio + HealthRatio);
                var cfg = ConfigService.instance.combatConfig.playerParam;
                if (HealthRatio * 100 < cfg.vignetteShowPercentage)
                {
                    svb.BlinkToValue(cfg.vignetteShowAlpha - HealthRatio);
                }

                OnHitFeedBack(v);
                camShakeBehaviour.Shake();
            }
        }

        public void PlayBlockFx()
        {
            blockPs.Play(true);
        }

        public void PlayBuffFx()
        {
            holylightPs.Play(true);
        }

        public void PlayHealSlow()
        {
            healPs.Play(true);
        }

        public void StopHealSlow()
        {
            healPs.Stop(true);
        }

        public void PlayHealFx()
        {
            healOncePs.Play(true);
        }
    }
}