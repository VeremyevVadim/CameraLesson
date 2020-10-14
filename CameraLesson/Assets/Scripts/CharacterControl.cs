using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    
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
    
    private void FixedUpdate()
    {
        _characterTransform.Translate(_movementVector * (movementSpeed * Time.fixedDeltaTime));
    }
}
