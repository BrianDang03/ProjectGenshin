using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player player { get; }
        public PlayerStateReusableData reusableData { get; }
        public PlayerIdlingState idlingState { get; }

        public PlayerWalkingState walkingState { get; }

        public PlayerRunningState runningState { get; }

        public PlayerSprintingState sprintingState { get; }

        public PlayerMovementStateMachine(Player aPlayer)
        {
            player = aPlayer;
            reusableData = new PlayerStateReusableData();

            idlingState = new PlayerIdlingState(this);

            walkingState = new PlayerWalkingState(this);

            runningState = new PlayerRunningState(this);

            sprintingState = new PlayerSprintingState(this);
        }
    }
}
