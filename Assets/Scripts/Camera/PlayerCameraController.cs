using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [Header("CameraSetting")]
    [SerializeField] float mouseSensivity = 0.1f;
    [SerializeField] float maxVerticalAngle = 60.0f;
    [SerializeField] float minVerticalAngle = -30.0f;
    [SerializeField]Transform camera;
    LayerMask cameraCollision;

    Transform playerPos;


    Vector2 _lookInput;

    float _yaw;
    float _pitch;



    [SerializeField] Vector3 _offset = new Vector3(0, 2, -3.5f);

    InputSystem_Actions cameraInput;

    public Vector3 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    public float MouseSensitivity
    {
        get => mouseSensivity;
        set => mouseSensivity = value;
    }

    private void Awake()
    {
       
        playerPos = transform.parent;
        cameraInput = new InputSystem_Actions();
        cameraCollision = LayerMask.GetMask("CameraCollision");
    }

    private void OnEnable()
    {
        cameraInput.Player.Look.performed += OnLook;
        cameraInput.Player.Look.canceled += OnLookCanceled;
        cameraInput.Player.Enable();
    }

    private void OnDisable()
    {
        cameraInput.Player.Look.performed -= OnLook;
        cameraInput.Player.Look.canceled -= OnLookCanceled;
        cameraInput.Player.Disable();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _lookInput = Vector2.zero;
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    void HandleCamera()
    {
        float mouseX = _lookInput.x * mouseSensivity;
        float mouseY = _lookInput.y * mouseSensivity;

        _yaw += mouseX;

        _pitch = Mathf.Clamp(_pitch - mouseY, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);


        Vector3 desiredPosition = playerPos.position + rotation * _offset;
        float desiredDistance = _offset.magnitude;


        Vector3 rayDir = (desiredPosition - playerPos.position).normalized;
        if (Physics.Raycast(playerPos.position, rayDir, out RaycastHit hit, desiredDistance, cameraCollision))
        {
            float buffer = 0.1f;
            desiredPosition = hit.point - rayDir * buffer;
        }

        transform.position = desiredPosition;

        transform.LookAt(playerPos.position + Vector3.up * 1.2f);
    }
}
