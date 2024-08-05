using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData;

        private float startTime;

        private bool keepSprinting;

        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.sprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.reusableData.movementSpeedModifier = sprintData.speedModifier;

            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (keepSprinting)
            {
                return;
            }

            if (Time.time < startTime + sprintData.sprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }

        public override void Exit()
        {
            base.Exit();
            keepSprinting = false;
        }
        #endregion

        #region Main Methods
        private void StopSprinting()
        {
            if (stateMachine.reusableData.movementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.idlingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.runningState);
        }
        #endregion

        #region Reusale Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.player.input.playerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();
            stateMachine.player.input.playerActions.Sprint.performed -= OnSprintPerformed;
        }
        #endregion

        #region Input Methods
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
        }
        #endregion
    }
}
