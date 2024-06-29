using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class Enemy : Core
    {
        [Header("Enemy States")]
        public Patrol patrol;
        public Chase chase;
        public Search search;
        public SearchWeapon searchWeapon;
        public Attack attack;

        [Header("Enemy Data")]
        public float attackDistance = 2;
        public Transform currentTarget;
        public List<Transform> weapons;
        public List<Transform> targets;
        public Collider2D[] obstacles;
        public int GetTargetsCount() => targets?.Count ?? 0;
        public List<SteeringBehaviorBase> steeringBehaviours;
        public ContextSolver movementDirectionSolver;

        [Header("Enemy Detectors Data")]
        public float detectionDelay = 0.05f;
        private float _detectionTimer;
        public List<Detector> detectors;
        public Detector weaponDetector;

        private void Start()
        {
            SetupInstances();
            Machine.Set(patrol);
        }

        private void Update()
        {
            //Debug.Log(CurrentState);
            SelectState();
            CurrentState.DoBranch();
        }

        private void FixedUpdate()
        {
            PerformDetection();
            CurrentState.FixedDoBranch();
        }

        private void SelectState()
        {
            if (!CurrentState.IsComplete && CurrentState != patrol) return;

            if (GetTargetsCount() > 0)
            {
                if (!gunDetector.isGunEquipped)
                    Machine.Set(searchWeapon);
                currentTarget = targets[0];
            }
            switch (CurrentState)
            {
                case Patrol:
                    if (currentTarget && currentTarget.CompareTag("Player"))
                        Machine.Set(chase);
                    break;
                case Chase:
                    if (currentTarget && currentTarget.CompareTag("Player"))
                        Machine.Set(attack);
                    else
                        Machine.Set(search);
                    break;
                case Search:
                    Machine.Set(patrol);
                    break;
                case SearchWeapon:
                    if (gunDetector.isGunEquipped)
                        Machine.Set(chase);
                    break;
                case Attack:
                    Machine.Set(chase);
                    break;
            }
        }

        private void PerformDetection()
        {
            _detectionTimer += Time.fixedDeltaTime;
            if (_detectionTimer < detectionDelay)
                return;

            foreach (var detector in detectors)
            {
                detector.Detect(this);
            }
            _detectionTimer = 0;
        }

        public bool CloseEnough()
        {
            return Vector2.Distance(transform.position, currentTarget.position) < attackDistance;
        }

        public void UpdateAttackDistance(float detectionRange)
        {
            attackDistance = attackDistance > detectionRange ? detectionRange : attackDistance;
        }

        // public void DisableParts()
        // {
        //     if (head != null)
        //     {
        //         head.SetActive(false);
        //     }
        // }
    }
}