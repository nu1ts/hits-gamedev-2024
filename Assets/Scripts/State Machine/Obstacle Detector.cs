using UnityEngine;
using System.Collections.Generic;

namespace State_Machine
{
    public class ObstacleDetector : Detector
    {
        [Header("Detection Settings")]
        public float detectionRadius = 2;
        public LayerMask layerMask;

        [Header("Gizmos Settings")]
        public bool showGizmos = true;

        private Collider2D[] _colliders = new Collider2D[10];

        public override void Detect(Enemy enemy)
        {
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, _colliders, layerMask);

            if (count >= _colliders.Length)
            {
                _colliders = new Collider2D[_colliders.Length * 2];
                count = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, _colliders, layerMask);
            }

            if (count > 0)
            {
                var validColliders = new List<Collider2D>(count);
                for (var i = 0; i < count; i++)
                {
                    validColliders.Add(_colliders[i]);
                }

                enemy.obstacles = validColliders.ToArray();
            }
            else
            {
                enemy.obstacles = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos || !Application.isPlaying || _colliders == null)
                return;

            Gizmos.color = Color.red;
            foreach (var obstacleCollider in _colliders)
            {
                if (obstacleCollider != null)
                {
                    Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
                }
            }
        }
    }
}