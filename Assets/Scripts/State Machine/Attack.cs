using UnityEngine;

namespace State_Machine
{
    public class Attack : State
    {
        [Header("Enemy Data")]
        public Enemy enemy;
        
        [Header("Animation")]
        public State anim;
        
        public override void Enter()
        {
            Set(anim);
        }

        public override void Do()
        {
            if (!enemy.currentTarget || !enemy.CloseEnough()) IsComplete = true;
            
            core.gunDetector.aiWeaponController.SetFireState(true);
        }
        
        protected override void FixedDo()
        {
            core.RotateTowardsTarget(core.transform, enemy.currentTarget.position, enemy.chase.rotationSpeed);
        }

        public override void Exit()
        {
            core.gunDetector.aiWeaponController.SetFireState(false);
        }
    }
}