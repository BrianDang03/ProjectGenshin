using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO data { get; private set; }
        public Rigidbody rb { get; private set; }

        public Transform mainCameraTransform { get; private set; }

        public PlayerInput input { get; private set; }
        private PlayerMovementStateMachine movementStateMachine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();
            movementStateMachine = new PlayerMovementStateMachine(this);
            mainCameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            movementStateMachine.ChangeState(movementStateMachine.idlingState);
        }

        private void Update()
        {
            movementStateMachine.HandleInput();
            movementStateMachine.Update();
        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate();
        }
    }
}
