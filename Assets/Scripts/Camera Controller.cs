using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraDistance = 3.5f;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 _target, _mousePosition, _currentVelocity;
    private float _zStart;
    private Camera _cam;
    
    private void Start()
    {
        _target = player.position;
        _zStart = transform.position.z;
        _cam = Camera.main;
    }
    
    private void LateUpdate()
    {
        _mousePosition = CaptureMousePosition();
        _target = UpdateTargetPosition();
        UpdateCameraPosition();
    }
    
    private Vector3 CaptureMousePosition()
    {
        Vector2 mousePosition = _cam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        mousePosition *= 2;
        mousePosition -= Vector2.one;
        const float max = 0.9f;
        if(Math.Abs(mousePosition.x) > max || Math.Abs(mousePosition.y) > max)
        {
            mousePosition = mousePosition.normalized;
        }

        return mousePosition;
    }
    
    private Vector3 UpdateTargetPosition()
    {
        var mouseOffset = _mousePosition * cameraDistance;
        var targetPosition  = player.position + mouseOffset;
        targetPosition .z = _zStart;
        
        return targetPosition;
    }
    
    private void UpdateCameraPosition()
    {
        var newPosition = Vector3.SmoothDamp(
            transform.position, _target,
            ref _currentVelocity, smoothTime
        );

        transform.position = newPosition;
    }
}
