using UnityEngine;

namespace State_Machine
{
    public class Search : State
    {
        [Header("Enemy Data")]
        public Enemy enemy;

        [Header("Animation")]
        public State anim;
        public State runAnim;

        [Header("Search Settings")]
        public float searchDuration = 5f;
        public float rotationSpeed = 5f;
        public float rotationInterval = 0.6f;
        public float maxRotationAngle = 45f;

        private float _rotationTimer;
        private Vector2 _currentDirection;
        private Vector2 _direction;
        private float _lastRandomAngle;

        public override void Enter()
        {
            Set(anim);
            _rotationTimer = 0;
            _currentDirection = -transform.up;
            _direction = _currentDirection;
        }

        public override void Do()
        {
            if (time >= searchDuration)
                IsComplete = true;
            else if (enemy.GetTargetsCount() > 0)
                IsComplete = true;
        }

        protected override void FixedDo()
        {
            _rotationTimer += Time.fixedDeltaTime;

            if (_rotationTimer >= rotationInterval)
            {
                RotateRandomly();
                _rotationTimer = 0;
            }

            enemy.RotateTowardsDirection(_direction, rotationSpeed);
        }

        public override void Exit()
        {
            
        }

        private void RotateRandomly()
        {
            float randomAngle;
            do randomAngle = Random.Range(-maxRotationAngle, maxRotationAngle);
            while (Mathf.Abs(randomAngle - _lastRandomAngle) < 10f);

            _lastRandomAngle = randomAngle;

            var angleInRadians = randomAngle * Mathf.Deg2Rad;
            var newDirection = new Vector2(
                _currentDirection.x * Mathf.Cos(angleInRadians) - _currentDirection.y * Mathf.Sin(angleInRadians),
                _currentDirection.x * Mathf.Sin(angleInRadians) + _currentDirection.y * Mathf.Cos(angleInRadians)
            );

            _direction = newDirection;
        }
    }
}