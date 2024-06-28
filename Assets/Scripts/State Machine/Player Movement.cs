using UnityEngine;
using UnityEngine.InputSystem;

namespace State_Machine
{
    public class PlayerMovement : Core
    {
        [Header("Player States")]
        public Idle idle;
        public Run run;
        
        [Header("Player Settings")]
        public float speed = 30f;
        public float rotationSpeed = 5f;
        public float acceleration;
        
        private Camera _cam;
        private Vector2 Input { get; set; }

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Start()
        {
            SetupInstances();
            Machine.Set(idle);
        }

        private void Update()
        {
            SelectState();
            Machine.State.Do();
        }

        private void FixedUpdate()
        {
            HandleMovement();

            var mousePosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RotateTowardsTarget(transform,mousePosition, rotationSpeed);
        }

        private void SelectState()
        {
            if (Input == Vector2.zero) Machine.Set(idle);
            else Machine.Set(run);
        }
        
        private void HandleMovement()
        {
            var targetPosition = (Vector2)transform.position + Input;
            MoveFromTo(transform.position, targetPosition, speed, acceleration);
        }
        
        private void OnMovement(InputValue inputValue)
        {
            Input = inputValue.Get<Vector2>();
        }
    }
}