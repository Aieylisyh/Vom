using com;
using UnityEngine;

namespace game
{
    public class PlayerDeath : UnitDeath
    {
        public BlinkCanvasGroup blinkCanvasGroup;
        public string explosionId;
        public float shakeTime = 0;

        public override void ResetState()
        {
            base.ResetState();
            blinkCanvasGroup.Stop();
            //Debug.LogWarning("PlayerDeath ResetState");
        }

        protected override void DieUnsilent()
        {
            //Debug.LogWarning("PlayerDeath DieUnsilent");
            Vibration.VibrateShort();
            PlayerDead();

            if (shakeTime > 0)
            {
                CameraControllerBehaviour.instance.ShakeCombatCam(shakeTime);
            }
        }

        protected override void DieSilent()
        {
            Debug.LogWarning("PlayerDeath DieSilent");
            PlayerDead();
        }

        private void PlayerDead()
        {
            blinkCanvasGroup.Play();
            PlayerMove pm = self.move as PlayerMove;
            pm.StartSink();
            SpawnEffect(explosionId);
            GameFlowService.instance.EnqueueEvent(GameFlowService.GameFlowEvent.OnPlayerDead);
        }
    }
}
