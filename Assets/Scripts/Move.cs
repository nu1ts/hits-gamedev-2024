using System.Linq;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    protected Rigidbody2D Rigidbody2D;

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected void MoveFromTo(Vector2 from, Vector2 to, float speed, float acceleration)
    {
        var direction = (to - from).normalized;
        Rigidbody2D.velocity = Vector2.MoveTowards(Rigidbody2D.velocity, direction * speed, acceleration * Time.fixedDeltaTime / Time.timeScale);
    }

    protected static void RotateTowardsTarget(Vector2 rotationTarget, Transform lookTarget, float rotationSpeed)
    {
        if (!lookTarget) return;

        var direction = new Vector2(rotationTarget.x - lookTarget.position.x, rotationTarget.y - lookTarget.position.y);

        var targetRotation = Quaternion.LookRotation(Vector3.forward, -direction);
        lookTarget.rotation = Quaternion.Lerp(lookTarget.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime / Time.timeScale);
    }

    
    protected static bool HasParameter(int paramHash, Animator animator)
    {
        return animator.parameters.Any(param => param.nameHash == paramHash);
    }
}