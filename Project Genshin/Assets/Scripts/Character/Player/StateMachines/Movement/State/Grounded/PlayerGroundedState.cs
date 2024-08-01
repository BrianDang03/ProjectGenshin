using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();
            stateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;
        }

        protected virtual void OnMove()
        {
            if (stateMachine.reusableData.shouldWalk)
            {
                stateMachine.ChangeState(stateMachine.walkingState);
                return;
            }

            stateMachine.ChangeState(stateMachine.runningState);
        }
        #endregion

        #region Input Methods
        protected virtual void OnMovementCanceled(InputAction.CallbackContext obj)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        #endregion
    }
}
