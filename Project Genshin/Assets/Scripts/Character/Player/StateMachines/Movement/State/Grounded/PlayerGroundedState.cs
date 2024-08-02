using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        private SlopeData slopeData;
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            slopeData = stateMachine.player.colliderUtility.slopeData;
        }

        #region IState Methods
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            FloatCapsule();
        }
        #endregion

        #region Main Methods
        private void FloatCapsule()
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.floatRayDistance, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
            {
                float distanceToFloatingPoint = stateMachine.player.colliderUtility.capsuleColliderData.colliderCenterInLocalSpace.y * stateMachine.player.transform.localScale.y - hit.distance;
                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                stateMachine.player.rb.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }
        #endregion

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
