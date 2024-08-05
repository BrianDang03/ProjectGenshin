using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerRunningState : PlayerMovingState
    {
        private PlayerSprintData sprintData;
        private float startTime;
        public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.sprintData;
        }

        #region IState Method
        public override void Enter()
        {
            base.Enter();
            stateMachine.reusableData.movementSpeedModifier = movementData.runData.speedModifier;
            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (!stateMachine.reusableData.shouldWalk)
            {
                return;
            }

            if (Time.time < startTime + sprintData.runToWalkTIme)
            {
                return;
            }

            StopRunning();
        }
        #endregion

        #region Main Methods
        private void StopRunning()
        {
            if (stateMachine.reusableData.movementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.idlingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.walkingState);
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
