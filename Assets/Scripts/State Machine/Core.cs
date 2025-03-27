using UnityEngine;

namespace State_Machine
{
    public class Core : MonoBehaviour
    {
        [Header("Core Fields")]
        public Rigidbody2D body;

        public Animator animator;

        public GunDetector gunDetector;

        protected StateMachine Machine;
        protected State CurrentState => Machine.State;

        protected void SetupInstances()
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            Machine = new StateMachine();

            var allChildStates = GetComponentsInChildren<State>();
            foreach (var element in allChildStates)
            {
                element.SetCore(this);
            }
        }
        
        public void MoveFromTo(Vector2 from, Vector2 to, float speed, float acceleration = 100f)
        {
            var direction = (to - from).normalized;
            body.velocity = Vector2.MoveTowards(body.velocity, direction * speed, acceleration * Time.fixedDeltaTime);
        }
        
        public void MoveInDirection(Vector2 direction, float speed, float acceleration = 100f)
        {
            body.velocity = Vector2.MoveTowards(body.velocity, direction * speed, acceleration * Time.fixedDeltaTime);
        }

        public void RotateTowardsTarget(Transform objectToRotate, Vector2 targetPosition, float rotationSpeed)
        {
            if (!objectToRotate) return;
            
            var directionToTarget = new Vector2(targetPosition.x - objectToRotate.position.x, targetPosition.y - objectToRotate.position.y);
            
            var targetRotation = Quaternion.LookRotation(Vector3.forward, -directionToTarget);
            
            objectToRotate.rotation = Quaternion.Lerp(objectToRotate.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        
        public void RotateTowardsDirection(Vector2 direction, float rotationSpeed)
        {
            var targetRotation = Quaternion.LookRotation(Vector3.forward, -direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}