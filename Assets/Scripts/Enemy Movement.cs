using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform head;
    [SerializeField] private Transform arms;
    [SerializeField] private Transform legs;
    [SerializeField] private float headRotationSpeed = 5f;
    [SerializeField] private float armsRotationSpeed = 8f;
    [SerializeField] private float legsRotationSpeed = 5f;
    [SerializeField] private Animator leftArmAnimator;
    [SerializeField] private Animator rightArmAnimator;
    [SerializeField] private Animator leftLegAnimator;
    [SerializeField] private Animator rightLegAnimator;
    
    private Rigidbody2D _rigidbody2D;
    private int _isMovingHash;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _isMovingHash = Animator.StringToHash("isMoving");

        // Найти игрока в сцене, если не был присвоен в инспекторе
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void FixedUpdate()
    {
        MoveEnemy();
        RotateTowardsPlayer(head, headRotationSpeed);
        RotateTowardsPlayer(arms, armsRotationSpeed);
        RotateTowardsPlayer(legs, legsRotationSpeed);
    }

    private void MoveEnemy()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found");
            return;
        }

        var direction = (player.position - transform.position).normalized;
        var distance = Vector2.Distance(player.position, transform.position);
        Vector2 input = distance > stoppingDistance ? direction : Vector2.zero;

        var isMoving = input != Vector2.zero;
        leftArmAnimator.SetBool(_isMovingHash, isMoving);
        rightArmAnimator.SetBool(_isMovingHash, isMoving);
        leftLegAnimator.SetBool(_isMovingHash, isMoving);
        rightLegAnimator.SetBool(_isMovingHash, isMoving);

        _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, input * speed, acceleration * Time.fixedDeltaTime);
    }

    private void RotateTowardsPlayer(Transform target, float rotationSpeed)
    {
        if (player == null)
            return;

        var direction = new Vector2(player.position.x - target.position.x, player.position.y - target.position.y);
        var targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        target.rotation = Quaternion.Lerp(target.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}
