using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData;

        private float startTime;

        private int consecutiveDashesUsed;

        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            dashData = movementData.dashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.reusableData.movementSpeedModifier = dashData.speedModifier;

            AddForceOnTransitionFromStationaryState();

            UpdateConsecutiveDashes();

            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (Time.time < startTime + 1f)
            {
                return;
            }

            stateMachine.ChangeState(stateMachine.sprintingState);
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();

            if (stateMachine.reusableData.movementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.idlingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.sprintingState);
        }
        #endregion

        #region Main Methods
        private void AddForceOnTransitionFromStationaryState()
        {
            if (stateMachine.reusableData.movementInput != Vector2.zero)
            {
                return;
            }

            Vector3 characterRotationDirection = stateMachine.player.transform.forward;

            characterRotationDirection.y = 0f;

            stateMachine.player.rb.velocity = characterRotationDirection * GetMovmentSpeed();
        }

        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if (consecutiveDashesUsed == dashData.consecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;

                stateMachine.player.input.DisableActionFor(stateMachine.player.input.playerActions.Dash, dashData.DashLimitReachedCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext obj)
        {
        }

        protected override void OnDashStarted(InputAction.CallbackContext obj)
        {
        }
        #endregion
    }
}
