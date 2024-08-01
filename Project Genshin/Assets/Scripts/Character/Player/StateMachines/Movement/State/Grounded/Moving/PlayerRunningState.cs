using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerRunningState : PlayerMovingState
    {
        public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        #region IState Method
        public override void Enter()
        {
            base.Enter();
            stateMachine.reusableData.movementSpeedModifier = movementData.runData.speedModifier;
        }
        #endregion

        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext obj)
        {
            base.OnWalkToggleStarted(obj);
            stateMachine.ChangeState(stateMachine.walkingState);
        }
        #endregion
    }
}
