using UnityEngine;

namespace State_Machine
{
    public class Chase : State
    {
        [Header("Enemy Data")]
        public Enemy enemy;
        
        [Header("Animation")]
        public State anim;
        
        [Header("Move Settings")]
        public float speed = 3;
        public float rotationSpeed = 10f;
        
        private Vector2 _direction;
        public override void Enter()
        {
            Set(anim);
        }

        public override void Do()
        {
            if (!enemy.currentTarget) 
                IsComplete = true;
            else if (enemy.CloseEnough())
                IsComplete = true;
        }
        
        protected override void FixedDo()
        {
            _direction = GetDirection();
            core.MoveInDirection(_direction, speed);
            core.RotateTowardsDirection(_direction, rotationSpeed);
        }

        public override void Exit()
        {
            Body.velocity = Vector2.zero;
        }

        private Vector2 GetDirection()
        {
            return enemy.movementDirectionSolver.GetDirectionToMove(enemy.steeringBehaviours, enemy);
        }
    }
}