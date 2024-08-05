using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerStateReusableData
    {
        //Stores data for code base uses
        public Vector2 movementInput { get; set; }
        public float movementSpeedModifier { get; set; } = 1f;
        public float movementOnSlopeSpeedModifier { get; set; } = 1f;
        public bool shouldWalk { get; set; }

        private Vector3 currentTargetRotation;
        private Vector3 timeToReachTargetRotation;
        private Vector3 dampedTargetRotationCurrentVelocity;
        private Vector3 dampedTargetRotationPassedTime;

        public ref Vector3 currentTargetRotationRef
        {
            get
            {
                return ref currentTargetRotation;
            }
        }

        public ref Vector3 timeToReachTargetRotationRef
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }

        public ref Vector3 dampedTargetRotationCurrentVelocityRef
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }

        public ref Vector3 dampedTargetRotationPassedTimeRef
        {
            get
            {
                return ref dampedTargetRotationPassedTime;
            }
        }
    }
}
