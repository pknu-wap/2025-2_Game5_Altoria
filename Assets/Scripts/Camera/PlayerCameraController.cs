using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("CameraSetting")]
    [SerializeField] float mouseSensivity = 0.1f;
    [SerializeField] float maxVerticalAngle = 60.0f;
    [SerializeField] float minVerticalAngle = -30.0f;
    [SerializeField] Transform camera;
    [SerializeField] LayerMask cameraCollision;
    [SerializeField] Vector3 _offset = new Vector3(0, 2, -3.5f);
    [SerializeField] PlayerInputHandler inputHandler; 

    Transform playerPos;

    Vector2 lookInput;
    float yaw;
    float pitch;



    void Awake()
    {
        playerPos = transform.parent;
        inputHandler = GetComponentInParent<PlayerInputHandler>();
    }

    void OnEnable()
    {
        if (inputHandler != null)
            inputHandler.OnLook += OnLook;
    }

    void OnDisable()
    {
        if (inputHandler != null)
            inputHandler.OnLook -= OnLook;
    }

    void OnLook(Vector2 delta)
    {
        lookInput = delta;
    }

    void LateUpdate()
    {
        HandleCamera();
    }

    void HandleCamera()
    {
        float mouseX = lookInput.x * mouseSensivity;
        float mouseY = lookInput.y * mouseSensivity;

        yaw += mouseX;
        pitch = Mathf.Clamp(pitch - mouseY, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
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
