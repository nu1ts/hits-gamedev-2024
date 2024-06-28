using UnityEngine;

namespace State_Machine
{
    public class SearchWeapon : State
    {
        [Header("Enemy Data")]
        public Enemy enemy;
        
        [Header("Animation")]
        public State anim;
        
        [Header("Child States")]
        public Navigate navigate;
        
        private Vector2 _direction;
        public override void Enter()
        {
            Set(anim, true);
            enemy.weaponDetector.Detect(enemy);
        }

        public override void Do()
        {
            if (core.gunDetector.isGunEquipped) IsComplete = true;
        }
        
        protected override void FixedDo()
        {
            _direction = GetDirection();
            core.MoveInDirection(_direction, navigate.speed);
            core.RotateTowardsDirection(_direction, navigate.rotationSpeed);
        }

        public override void Exit()
        {
            enemy.weapons.Clear();
        }
        
        private Vector2 GetDirection()
        {
            return enemy.movementDirectionSolver.GetDirectionToMove(enemy.steeringBehaviours, enemy);
        }
    }
}
