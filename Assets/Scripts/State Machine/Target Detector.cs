using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class TargetDetector : Detector
    {
        [Header("Detection Settings")]
        public float circleDetectionRange = 5f;
        public float fovDetectionAngle = 45f;
        public float fovDetectionRange = 5f;
        public LayerMask obstaclesLayerMask, playerLayerMask;
        public int maxTargets = 4;

        [Header("Gizmos Settings")]
        public bool showGizmos;

        private List<Transform> _colliders;
        private Collider2D[] _results;

        private void Awake()
        {
            _results = new Collider2D[maxTargets];
            _colliders = new List<Transform>();
        }

        public override void Detect(Enemy enemy)
        {
            enemy.UpdateAttackDistance(fovDetectionRange);

            _colliders.Clear();
            
            Vector2 forwardDirection = -transform.up;
            Vector2 origin = transform.position;

            var halfAngle = fovDetectionAngle / 2f;
            
            var count = Physics2D.OverlapCircleNonAlloc(origin, fovDetectionRange, _results, playerLayerMask);

            for (var i = 0; i < count; i++)
            {
                var playerCollider = _results[i];
                
                var directionToTarget = ((Vector2)playerCollider.transform.position - origin).normalized;
                var angleToTarget = Vector2.Angle(forwardDirection, directionToTarget);

                if (angleToTarget <= halfAngle)
                {
                    var hit = Physics2D.Raycast(origin, directionToTarget, fovDetectionRange, obstaclesLayerMask);

                    if (!hit.collider || (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Debug.DrawRay(origin, directionToTarget * fovDetectionRange, Color.magenta);
                        _colliders.Add(playerCollider.transform);
                    }
                }
                else if (Vector2.Distance(origin, playerCollider.transform.position) <= circleDetectionRange)
                {
                    var hit = Physics2D.Raycast(origin, directionToTarget, circleDetectionRange, obstaclesLayerMask);

                    if (!hit.collider || (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Debug.DrawRay(origin, directionToTarget * circleDetectionRange, Color.magenta);
                        _colliders.Add(playerCollider.transform);
                    }
                }
            }
            
            enemy.targets = _colliders.Count > 0 ? _colliders : null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmos)
                return;

            Gizmos.DrawWireSphere(transform.position, fovDetectionRange);
            Gizmos.DrawWireSphere(transform.position, circleDetectionRange);
            
            var forwardDirection = -transform.up;
            var halfAngle = fovDetectionAngle / 2f;
            var leftRotation = Quaternion.Euler(0, 0, -halfAngle);
            var rightRotation = Quaternion.Euler(0, 0, halfAngle);

            var leftBoundary = leftRotation * forwardDirection * fovDetectionRange;
            var rightBoundary = rightRotation * forwardDirection * fovDetectionRange;

            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.position, leftBoundary);
            Gizmos.DrawRay(transform.position, rightBoundary);

            if (_colliders == null)
                return;
            Gizmos.color = Color.magenta;
            foreach (var item in _colliders)
            {
                Gizmos.DrawSphere(item.position, 0.3f);
            }
        }
    }
}