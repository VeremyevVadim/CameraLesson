using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float mouseSensibility = 45f;
    [SerializeField] private float cameraLimitY = 90f;

    private float _x = 0f;
    private float _y = 0f;
    private Transform _characterTransform;
    
    private Camera[] _cameras;
    private Transform _gunCamera;
    
    private void Start()
    {
        _characterTransform = transform;
        cameraLimitY = Mathf.Abs(cameraLimitY);
        _cameras = GameObject.FindObjectsOfType<Camera>();
    }

    private Vector3 _movementVector
    {
        get
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            return new Vector3(horizontal, 0.0f, vertical);
        }
    }
    
    private Vector3 _mouseMovementVector
    {
        get
        {
            var horizontal = Input.GetAxis("Mouse X");
            var vertical = Input.GetAxis("Mouse Y");
            return new Vector3(horizontal, vertical, 0.0f);
        }
    }
    private void FixedUpdate()
    {
        _characterTransform.Translate(_movementVector * (movementSpeed * Time.fixedDeltaTime));
        
        var rotationSpeed = mouseSensibility * Time.fixedDeltaTime;
        var mouseMovementVector = _mouseMovementVector;
        _x = _characterTransform.localEulerAngles.y + mouseMovementVector.x * rotationSpeed;
        _y += mouseMovementVector.y * rotationSpeed;
        _y = Mathf.Clamp(_y, -cameraLimitY, cameraLimitY);
        _characterTransform.localEulerAngles = new Vector3(-_y, _x, 0.0f);
    }
}
