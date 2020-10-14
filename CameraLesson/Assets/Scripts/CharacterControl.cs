using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float mouseSensibility = 45f;
    
    private Transform _characterTransform;
    private void Start()
    {
        _characterTransform = transform;
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
            return new Vector3(horizontal, 0.0f, 0.0f);
        }
    }
    private void FixedUpdate()
    {
        _characterTransform.Translate(_movementVector * (movementSpeed * Time.fixedDeltaTime));
        _characterTransform.rotation *= Quaternion.Euler(
            0,
            _mouseMovementVector.x * mouseSensibility * Time.fixedDeltaTime,
            0
            );
        
    }
}
