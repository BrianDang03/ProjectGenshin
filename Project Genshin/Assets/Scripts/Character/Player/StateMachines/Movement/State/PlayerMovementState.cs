using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine movementStateMachine;

        protected Vector2 movementInput;

        protected float baseSpeed = 5f;
        protected float speedModifier = 1f;

        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            movementStateMachine = playerMovementStateMachine;
        }

        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);
        }

        public virtual void Exit()
        {

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
            movementInput = movementStateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (movementInput == Vector2.zero || speedModifier == 0f)
            {
                return;
            }

            Vector3 moveDir = GetMovementInputDirection();
            float movementSpeed = GetMovmentSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            movementStateMachine.player.rb.AddForce(moveDir * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }
        #endregion

        #region Resuable Methods
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(movementInput.x, 0, movementInput.y);
        }

        protected float GetMovmentSpeed()
        {
            return baseSpeed * speedModifier;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = movementStateMachine.player.rb.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }
        #endregion

    }
}
