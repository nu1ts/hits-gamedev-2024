using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class ObstacleAvoidanceBehavior : SteeringBehaviorBase
    {
        public float radius = 2f;
        public BoxCollider2D agentCollider;

        [SerializeField]
        private bool showGizmo = true;
        
        private float[] _dangersResultTemp;

        public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, Enemy enemy)
        {
            if (enemy.obstacles == null || enemy.obstacles.Length == 0)
            {
                return (danger, interest);
            }

            foreach (var obstacleCollider in enemy.obstacles)
            {
                if (!obstacleCollider) continue;

                var directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
                var distanceToObstacle = directionToObstacle.magnitude;
                
                var weight = distanceToObstacle <= agentCollider.size.x
                    ? 1
                    : (radius - distanceToObstacle) / radius;

                var directionToObstacleNormalized = directionToObstacle.normalized;
                
                for (var i = 0; i < DirectionHelper.EightDirections.Count; i++)
                {
                    var result = Vector2.Dot(directionToObstacleNormalized, DirectionHelper.EightDirections[i]);
                    var valueToPutIn = result * weight;
                    
                    if (valueToPutIn > danger[i])
                    {
                        danger[i] = valueToPutIn;
                    }
                }
            }
            _dangersResultTemp = danger;
            return (danger, interest);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || _dangersResultTemp == null)
                return;

            Gizmos.color = Color.red;
            for (var i = 0; i < _dangersResultTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, DirectionHelper.EightDirections[i] * _dangersResultTemp[i] * 2);
            }
        }
    }

    public static class DirectionHelper
    {
        public static readonly List<Vector2> EightDirections = new()
        {
            new Vector2(0, 1).normalized,
            new Vector2(1, 1).normalized,
            new Vector2(1, 0).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(0, -1).normalized,
            new Vector2(-1, -1).normalized,
            new Vector2(-1, 0).normalized,
            new Vector2(-1, 1).normalized
        };
    }
}