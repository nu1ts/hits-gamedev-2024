using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterArmController : MonoBehaviour
{
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform playerBody;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector2 _input;

    private void Update()
    {
        RotateArms();
    }

    private void OnMovement(InputValue inputValue)
    {
        _input = inputValue.Get<Vector2>();
    }

    private void RotateArms()
    {
        if (_input.sqrMagnitude <= 0.1f)
        {
            return;
        }
        
        var angle = Mathf.Atan2(_input.y, _input.x) * Mathf.Rad2Deg;
        
        leftArm.rotation = Quaternion.Lerp(leftArm.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
        rightArm.rotation = Quaternion.Lerp(rightArm.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
    }
}
