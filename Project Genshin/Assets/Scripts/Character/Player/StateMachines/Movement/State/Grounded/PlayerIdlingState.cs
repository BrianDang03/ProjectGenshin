using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        #region IState Method
        public override void Enter()
        {
            base.Enter();
            stateMachine.reusableData.movementSpeedModifier = 0f;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if (stateMachine.reusableData.movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }
        #endregion
    }
}
