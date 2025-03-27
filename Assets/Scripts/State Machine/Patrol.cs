using UnityEngine;

namespace State_Machine
{
    public class Patrol : State
    {
        [Header("Child States")]
        public Idle idle;
        public Navigate navigate;
        
        [Header("Patrol Settings")]
        public float radius;
        
        [Header("Idle Time")]
        public float idleTime = 5f;

        private Vector2 _currentCenter;

        public override void Enter()
        {
            _currentCenter = transform.position;
            GoToNextDestination();
        }

        public override void Do()
        {
            if (CurrentState == navigate)
            {
                if (!navigate.IsComplete) return;
                
                Body.velocity = Vector2.zero;
                Set(idle, true);
            }
            else if (CurrentState.time > idleTime)
            {
                GoToNextDestination();
            }
        }

        public override void Exit()
        {
            Body.velocity = Vector2.zero;
        }

        private void GoToNextDestination()
        {
            navigate.destination = GetRandomPointAround();
            Set(navigate, true);
        }
        
        private Vector2 GetRandomPointAround()
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
            var distance = Random.Range(0f, radius);
            
            var x = _currentCenter.x + distance * Mathf.Cos(angle);
            var y = _currentCenter.y + distance * Mathf.Sin(angle);
        
            return new Vector2(x, y);
        }
    }
}