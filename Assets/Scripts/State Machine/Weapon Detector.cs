using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class WeaponDetector : Detector
    {
        [Header("Detection Settings")]
        public float detectionRange = 100f;
        public LayerMask weaponLayerMask;

        [Header("Gizmos Settings")]
        public bool showGizmos;

        private List<Transform> _colliders;
        private Collider2D[] _results;

        private void Awake()
        {
            _colliders = new List<Transform>();
            _results = new Collider2D[50];
        }

        public override void Detect(Enemy enemy)
        {
            _colliders.Clear();
            
            Vector2 origin = transform.position;
            
            var count = Physics2D.OverlapCircleNonAlloc(origin, detectionRange, _results, weaponLayerMask);

            for (var i = 0; i < count; i++)
            {
                var weaponCollider = _results[i];
                _colliders.Add(weaponCollider.transform);
                
                Debug.DrawRay(origin, (weaponCollider.transform.position - transform.position).normalized * detectionRange, Color.blue);
            }
            
            enemy.weapons = _colliders.Count > 0 ? _colliders : null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmos)
                return;

            Gizmos.DrawWireSphere(transform.position, detectionRange);

            if (_colliders == null)
                return;
            
            Gizmos.color = Color.blue;
            foreach (var item in _colliders)
            {
                Gizmos.DrawSphere(item.position, 0.3f);
            }
        }
    }
}