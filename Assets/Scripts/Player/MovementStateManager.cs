using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{
    [SerializeField] private Transform CameraTransform;
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] float groundYOffset; // Ground Check 
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f; // Gravity
    [SerializeField] private bool shouldFaceMoveDirection = false;

    [HideInInspector] public Vector3 dir;
    float hzInput, vInput;

    CharacterController controller;
    PlayerInputActions inputAction; //Input Actions

    Vector3 spherePos;
    Vector3 velocity;
    Vector2 movementInput; // Move
    Vector2 lookPosition; // Attack

    void Awake()
    {
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        // inputAction.PlayerControls.Look.performed += ctx => lookPosition = ctx.ReadValue<Vector2>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = CameraTransform.forward;
        Vector3 right = CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * movementInput.y + right * movementInput.x;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if(shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        GetDirectionAndMove();
        Gravity();

    }

    void GetDirectionAndMove()
    {
        hzInput = movementInput.x;
        vInput = movementInput.y;

        dir = transform.forward * vInput + transform.right * hzInput;

        controller.Move(dir * moveSpeed * Time.deltaTime);

    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if(Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if(!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0) velocity.y = -2f; 

        controller.Move(velocity * Time.deltaTime);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    //}

    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
}
