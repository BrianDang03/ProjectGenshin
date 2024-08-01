using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        #region IState Method
        public override void Enter()
        {
            base.Enter();

            stateMachine.reusableData.movementSpeedModifier = movementData.walkData.speedModifier;
        }
        #endregion

        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext obj)
        {
            base.OnWalkToggleStarted(obj);
            stateMachine.ChangeState(stateMachine.runningState);
        }
        #endregion

    }
}
