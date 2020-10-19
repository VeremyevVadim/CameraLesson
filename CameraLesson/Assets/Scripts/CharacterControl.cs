using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

enum Player
{
    First,
    Second
}

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private Player playerNumber = Player.First;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float mouseSensibility = 45f;
    [SerializeField] private float cameraLimitY = 90f;
    [SerializeField] private float zoomDamping = 5.0f;
    private float _x = 0f;
    private float _y = 0f;

    private Transform _characterTransform;
    
    private Camera[] _cameras;
    private float _baseCameraFOV = 60f;
    private float _zoomCameraFOV = 30f;
    private bool _isZoom = false;

    private void Start()
    {
        _characterTransform = transform;
        cameraLimitY = Mathf.Abs(cameraLimitY);
        _cameras = FindObjectsOfType<Camera>();

        if (_cameras.Length == 0) return;
        _baseCameraFOV = _cameras[0].fieldOfView;
        _zoomCameraFOV = _baseCameraFOV / 2;
    }

    private Vector3 MovementVector
    {
        get
        {
            float horizontal, vertical;
            if (playerNumber == Player.First)
            {
                horizontal = Input.GetAxis("PlayerFirstHorizontal");
                vertical = Input.GetAxis("PlayerFirstVertical");
            }
            else
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }
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

    private void CharacterMove()
    {
        _characterTransform.Translate(MovementVector * (movementSpeed * Time.fixedDeltaTime));

        var position = _characterTransform.position;
        position -= new Vector3(0.0f, position.y, 0.0f);
        _characterTransform.position = position;
    }

    private void CameraRotate()
    {
        var rotationSpeed = mouseSensibility * Time.fixedDeltaTime;
        var mouseMovementVector = _mouseMovementVector;
        _x = _characterTransform.localEulerAngles.y + mouseMovementVector.x * rotationSpeed;
        _y += mouseMovementVector.y * rotationSpeed;
        _y = Mathf.Clamp(_y, -cameraLimitY, cameraLimitY);
        _characterTransform.localEulerAngles = new Vector3(-_y, _x, 0.0f);
    }

    private void CameraSmoothZoom()
    {
        var wantedZoom = _isZoom ? _zoomCameraFOV : _baseCameraFOV;
        
        foreach (var currentCamera in _cameras)
        {
            var currentFOV = currentCamera.fieldOfView;
            currentFOV = Mathf.Lerp(currentFOV, wantedZoom, zoomDamping * Time.deltaTime);
            currentCamera.fieldOfView = currentFOV;
        }
    }

    private void FixedUpdate()
    {
        CharacterMove();
        CameraRotate();
        if (Input.GetButton("Fire2"))
        {
            _isZoom = true;
        }

        CameraSmoothZoom();
        _isZoom = false;
    }
    
}
