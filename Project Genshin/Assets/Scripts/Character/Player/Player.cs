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

        [field: Header("Collisions")]
        [field: SerializeField] public CapsuleColliderUtility colliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData layerData { get; private set; }

        public Rigidbody rb { get; private set; }

        public Transform mainCameraTransform { get; private set; }

        public PlayerInput input { get; private set; }
        private PlayerMovementStateMachine movementStateMachine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();

            colliderUtility.Initialize(gameObject);
            colliderUtility.CalculateCapsuleColliderDimensions();

            movementStateMachine = new PlayerMovementStateMachine(this);
            mainCameraTransform = Camera.main.transform;
        }

        private void OnValidate()
        {
            colliderUtility.Initialize(gameObject);
            colliderUtility.CalculateCapsuleColliderDimensions();
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
