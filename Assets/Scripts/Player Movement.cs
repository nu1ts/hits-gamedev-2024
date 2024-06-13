using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private float headRotationSpeed = 5f;
    [SerializeField] private float armsRotationSpeed = 8f;
    [SerializeField] private float legsRotationSpeed = 5f;
    [SerializeField] private Transform head;
    [SerializeField] private Transform arms;
    [SerializeField] private Transform legs;
    [SerializeField] private Animator leftArmAnimator;
    [SerializeField] private Animator rightArmAnimator;
    [SerializeField] private Animator leftLegAnimator;
    [SerializeField] private Animator rightLegAnimator;
    
    private Vector2 _input;
    private Rigidbody2D _rigidbody2D;
    private Camera _cam;
    private int _isMovingHash;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        _isMovingHash = Animator.StringToHash("isMoving");
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        RotateTowardsMouse(head, headRotationSpeed);
        RotateTowardsMouse(arms, armsRotationSpeed);
        RotateTowardsMouse(legs, legsRotationSpeed);
    }
    
    private void MovePlayer()
    {
        var isMoving = _input != Vector2.zero;
        leftArmAnimator.SetBool(_isMovingHash, isMoving);
        rightArmAnimator.SetBool(_isMovingHash, isMoving);
        leftLegAnimator.SetBool(_isMovingHash, isMoving);
        rightLegAnimator.SetBool(_isMovingHash, isMoving);
        
        _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, _input * speed, acceleration * Time.deltaTime);
    }

    private void RotateTowardsMouse(Transform target, float rotationSpeed)
    {
        var mousePosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var direction = new Vector2(mousePosition.x - target.position.x, mousePosition.y - target.position.y);

        var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        target.rotation = Quaternion.Lerp(target.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
    
    private void OnMovement(InputValue inputValue)
    {
        _input = inputValue.Get<Vector2>();
    }
}