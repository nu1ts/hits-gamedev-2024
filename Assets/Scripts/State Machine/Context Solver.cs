using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class ContextSolver : MonoBehaviour
    {
        public bool showGizmos;
        
        private Vector2 _resultDirection = Vector2.zero;
        private const float RayLength = 2;
    
        public Vector2 GetDirectionToMove(List<SteeringBehaviorBase> behaviours, Enemy enemy)
        {
            var danger = new float[8];
            var interest = new float[8];
            
            foreach (var behaviour in behaviours)
            {
                (danger, interest) = behaviour.GetSteering(danger, interest, enemy);
            }
            
            for (var i = 0; i < 8; i++)
            {
                interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
            }
            
            var outputDirection = Vector2.zero;
            for (var i = 0; i < 8; i++)
            {
                outputDirection += DirectionHelper.EightDirections[i] * interest[i];
            }
    
            outputDirection.Normalize();
    
            _resultDirection = outputDirection;
            
            return _resultDirection;
        }
    
    
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !showGizmos)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, _resultDirection * RayLength);
        }
    }
}