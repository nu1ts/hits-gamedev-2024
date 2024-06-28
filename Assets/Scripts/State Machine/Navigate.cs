using UnityEngine;

namespace State_Machine
{
    public class Navigate : State
    {
        [Header("Animation")]
        public State anim;
        
        [Header("Move Settings")]
        public float speed = 3;
        public float rotationSpeed = 10f;
        
        [Header("Destination Settings")]
        public Vector2 destination;
        public float threshold = 0.1f;

        public override void Enter()
        {
            Set(anim);
        }

        public override void Do()
        {
            if (Vector2.Distance(core.transform.position, destination) < threshold)
                IsComplete = true;
        }

        protected override void FixedDo()
        {
            core.RotateTowardsTarget(core.transform, destination, rotationSpeed);
            core.MoveFromTo(core.transform.position, destination, speed);
        }
    }
}