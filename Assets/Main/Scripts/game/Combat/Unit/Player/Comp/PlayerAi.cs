﻿using UnityEngine;
using com;

namespace game
{
    public class PlayerAi : UnitAi
    {
        public float MoveInputThreshold = 0.1f;

        protected override void Tick()
        {
            if (!GameFlowService.instance.IsGameplayControlEnabled())
                return;

            MovePlayerShip();
        }

        void MovePlayerShip()
        {
            Vector2 delta = InputPanel.instance.GetDelta();
            float x = delta.x;
            // float y = delta.y;

            if (x > MoveInputThreshold)
            {
                self.move.MoveRight();
            }
            else if (x < -MoveInputThreshold)
            {
                self.move.MoveLeft();
            }
        }

        public override void ResetState()
        {
        }
    }
}