using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;

        protected PlayerGroundedData movementData;

        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;

            movementData = stateMachine.player.data.groundedData;

            InitializeData();
        }

        private void InitializeData()
        {
            stateMachine.reusableData.timeToReachTargetRotationRef = movementData.baseRotationData.targetRotationReachTime;
        }

        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);

            AddInputActionCallback();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallback();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {

        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }
        #endregion

        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.reusableData.movementInput = stateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (stateMachine.reusableData.movementInput == Vector2.zero || stateMachine.reusableData.movementSpeedModifier == 0f)
            {
                return;
            }

            Vector3 moveDir = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(moveDir);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovmentSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine.player.rb.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
            stateMachine.reusableData.currentTargetRotationRef.y = targetAngle;
            stateMachine.reusableData.dampedTargetRotationPassedTimeRef.y = 0f;
        }

        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += stateMachine.player.mainCameraTransform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }
        #endregion

        #region Resuable Methods
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.reusableData.movementInput.x, 0, stateMachine.reusableData.movementInput.y);
        }

        protected float GetMovmentSpeed()
        {
            return movementData.baseSpeed * stateMachine.reusableData.movementSpeedModifier;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine.player.rb.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.player.rb.velocity.y, 0f);
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine.player.rb.rotation.eulerAngles.y;

            if (currentYAngle == stateMachine.reusableData.currentTargetRotationRef.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.reusableData.currentTargetRotationRef.y, ref stateMachine.reusableData.dampedTargetRotationCurrentVelocityRef.y, stateMachine.reusableData.timeToReachTargetRotationRef.y - stateMachine.reusableData.dampedTargetRotationPassedTimeRef.y);

            stateMachine.reusableData.dampedTargetRotationPassedTimeRef.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            stateMachine.player.rb.MoveRotation(targetRotation);
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle != stateMachine.reusableData.currentTargetRotationRef.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        protected void ResetVelocity()
        {
            stateMachine.player.rb.velocity = Vector3.zero;
        }

        protected virtual void AddInputActionCallback()
        {
            stateMachine.player.input.playerActions.WalkToggle.started += OnWalkToggleStarted;
        }

        protected virtual void RemoveInputActionsCallback()
        {
            stateMachine.player.input.playerActions.WalkToggle.started -= OnWalkToggleStarted;
        }

        #endregion

        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext obj)
        {
            stateMachine.reusableData.shouldWalk = !stateMachine.reusableData.shouldWalk;
        }
        #endregion
    }
}
