using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MoveController
{
    public float speed = 3f;
    public float acceleration = 100f;
    public float stoppingDistance = 2f;
    public float headRotationSpeed = 5f;
    public float armsRotationSpeed = 8f;
    public float legsRotationSpeed = 5f;

    public Transform target;
    public Transform head;
    public Transform arms;
    public Transform legs;

    private int _isMovingHash;
    private List<Animator> _animators;

    protected override void Awake()
    {
        base.Awake();
        
        _isMovingHash = Animator.StringToHash("isMoving");
        
        if (target != null) return;
        
        var targetObj = GameObject.FindGameObjectWithTag("Player");
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        else
        {
            Debug.LogWarning("There isn't Player to attack");
        }
    }
    
    private void Start()
    {
        _animators = GetComponentsInChildren<Animator>().ToList();
    }

    private void FixedUpdate()
    { 
        MoveEnemy();
        RotateEnemy();
    }

    private void MoveEnemy()
    { 
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = target.position;
        
        var distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        var toPosition = distanceToTarget > stoppingDistance ? targetPosition : currentPosition;
        
        MoveFromTo(currentPosition, toPosition, speed, acceleration);

        var isMoving = toPosition != currentPosition;
        
        _animators
            .Where(animator => HasParameter(_isMovingHash, animator))
            .ToList()
            .ForEach(animator => animator.SetBool(_isMovingHash, isMoving));
    }

    private void RotateEnemy()
    {
        if(!target) return;
        Vector2 targetPosition = target.position;

        RotateTowardsTarget(targetPosition, head, headRotationSpeed);
        RotateTowardsTarget(targetPosition, arms, armsRotationSpeed);
        RotateTowardsTarget(targetPosition, legs, legsRotationSpeed);
    }
}