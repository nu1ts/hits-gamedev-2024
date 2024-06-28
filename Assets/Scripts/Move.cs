using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody2D _body;

    protected void Setup(Rigidbody2D body)
    {
        _body = body;
    }
    protected void MoveFromTo(Vector2 from, Vector2 to, float speed, float acceleration)
    {
        var direction = (to - from).normalized;
        _body.velocity = Vector2.MoveTowards(_body.velocity, direction * speed, acceleration * Time.fixedDeltaTime);
    }

    protected static void RotateTowardsTarget(Vector2 rotationTarget, Transform lookTarget, float rotationSpeed)
    {
        if (!lookTarget) return;

        var direction = new Vector2(rotationTarget.x - lookTarget.position.x, rotationTarget.y - lookTarget.position.y);

        var targetRotation = Quaternion.LookRotation(Vector3.forward, -direction);
        lookTarget.rotation = Quaternion.Lerp(lookTarget.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    
    protected static bool HasParameter(int paramHash, Animator animator)
    {
        return animator.parameters.Any(param => param.nameHash == paramHash);
    }
}