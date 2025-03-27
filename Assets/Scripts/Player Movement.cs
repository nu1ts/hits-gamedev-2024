// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerMovementOld : MonoBehaviour
// {
//     public float speed = 30f;
//     public float acceleration = 100f;
//     public float headRotationSpeed = 5f;
//     public float armsRotationSpeed = 8f;
//     public float legsRotationSpeed = 5f;

//     public GameObject head;
//     public GameObject arms;
//     public GameObject legs;

//     private Vector2 _input;
//     private Camera _cam;
//     private int _isMovingHash;
//     private List<Animator> _animators;

//     public PlayerDashController dashController;

//     protected void Awake()
//     {
//         _cam = Camera.main;
//         _isMovingHash = Animator.StringToHash("isMoving");
//     }

//     private void Start()
//     {
//         _animators = GetComponentsInChildren<Animator>().ToList();
//     }

//     private void FixedUpdate()
//     {
//         if (!dashController._isDashing)
//         {
//             MovePlayer();

//             var mousePosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
//             RotateTowardsTarget(mousePosition, head.transform, headRotationSpeed);
//             RotateTowardsTarget(mousePosition, arms.transform, armsRotationSpeed);
//             RotateTowardsTarget(mousePosition, legs.transform, legsRotationSpeed);
//         }
//     }

//     private void MovePlayer()
//     {
//         var isMoving = _input != Vector2.zero;

//         var targetPosition = (Vector2)transform.position + _input;
//         MoveFromTo(transform.position, targetPosition, speed / Time.timeScale, acceleration);

//         _animators
//             .Where(animator => HasParameter(_isMovingHash, animator))
//             .ToList()
//             .ForEach(animator => animator.SetBool(_isMovingHash, isMoving));
//     }

//     private void OnMovement(InputValue inputValue)
//     {
//         _input = inputValue.Get<Vector2>();
//     }

// private void OnDash()
//     {
//         if (dashController != null && dashController.CanDash())
//         {
//             dashController.StartDash(_input.normalized);
//         }
//     }
// }