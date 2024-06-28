using System.Linq;
using UnityEngine;

namespace State_Machine
{
    public class SeekTargetBehavior : SteeringBehaviorBase
    {
        public bool showGizmo = true;

        private bool _reachedLastTarget = true;

        private Vector2 _targetPositionCached;
        private float[] _interestsTemp;

        private float _threshold;
        public float playerThreshold = 1.5f;
        public float weaponThreshold = 2.0f;

        public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, Enemy enemy)
        {
            if (_reachedLastTarget)
            {
                if (enemy.targets == null || enemy.GetTargetsCount() == 0)
                {
                    enemy.currentTarget = null;
                    return (danger, interest);
                }
                
                _reachedLastTarget = false;
                enemy.currentTarget = enemy.weapons.Any() 
                    ? enemy.weapons.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault()
                    : enemy.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
            }

            if (enemy.currentTarget && 
                enemy.targets != null && 
                (enemy.targets.Contains(enemy.currentTarget) || enemy.weapons.Contains(enemy.currentTarget))
                )
            {
                _targetPositionCached = enemy.currentTarget.position;
                _threshold = enemy.currentTarget.CompareTag("Player") ? playerThreshold : weaponThreshold;
            }
            
            if (Vector2.Distance(transform.position, _targetPositionCached) < _threshold)
            {
                _reachedLastTarget = true;
                enemy.currentTarget = null;
                return (danger, interest);
            }
            
            var directionToTarget = _targetPositionCached - (Vector2)transform.position;
            for (var i = 0; i < interest.Length; i++)
            {
                var result = Vector2.Dot(directionToTarget.normalized, DirectionHelper.EightDirections[i]);
                
                if (result > 0)
                {
                    var valueToPutIn = result;
                    if (valueToPutIn > interest[i])
                    {
                        interest[i] = valueToPutIn;
                    }

                }
            }
            _interestsTemp = interest;
            return (danger, interest);
        }

        private void OnDrawGizmos()
        {

            if (showGizmo == false)
                return;
            Gizmos.DrawSphere(_targetPositionCached, 0.2f);

            if (Application.isPlaying && _interestsTemp != null)
            {
                if (_interestsTemp != null)
                {
                    Gizmos.color = Color.green;
                    for (var i = 0; i < _interestsTemp.Length; i++)
                    {
                        Gizmos.DrawRay(transform.position, DirectionHelper.EightDirections[i] * _interestsTemp[i]*2);
                    }
                    if (_reachedLastTarget == false)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(_targetPositionCached, 0.1f);
                    }
                }
            }
        }
    }
}
