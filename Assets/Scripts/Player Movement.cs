using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 30f;
    public float acceleration = 100f;
    public float headRotationSpeed = 5f;
    public float armsRotationSpeed = 8f;
    public float legsRotationSpeed = 5f;
    public Vector2 crosshairOffset;
    public GameObject head;
    public GameObject arms;
    public GameObject legs;
    
    public Animator leftArmAnimator;
    public Animator rightArmAnimator;
    public Animator leftLegAnimator;
    public Animator rightLegAnimator;
    
    private Vector2 _input;
    private Rigidbody2D _rigidbody2D;
    private Camera _cam;
    private int _isMovingHash;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        _isMovingHash = Animator.StringToHash("isMoving");
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        if(head) RotateTowardsMouse(head.transform, headRotationSpeed);
        if(arms) RotateTowardsMouse(arms.transform, armsRotationSpeed);
        if(legs) RotateTowardsMouse(legs.transform, legsRotationSpeed);
    }
    
    private void MovePlayer()
    {
        var isMoving = _input != Vector2.zero;
        if(leftArmAnimator) leftArmAnimator.SetBool(_isMovingHash, isMoving);
        if(rightArmAnimator) rightArmAnimator.SetBool(_isMovingHash, isMoving);
        if(leftLegAnimator) leftLegAnimator.SetBool(_isMovingHash, isMoving);
        if(rightLegAnimator) rightLegAnimator.SetBool(_isMovingHash, isMoving);
        
        _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, _input * speed, acceleration * Time.deltaTime);
    }

    private void RotateTowardsMouse(Transform target, float rotationSpeed)
    {
        var mousePosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var direction = new Vector2(mousePosition.x - target.position.x, mousePosition.y - target.position.y);

        direction += crosshairOffset;

        var targetRotation = Quaternion.LookRotation(Vector3.forward, -direction);
        target.rotation = Quaternion.Lerp(target.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
    
    private void OnMovement(InputValue inputValue)
    {
        _input = inputValue.Get<Vector2>();
    }
}