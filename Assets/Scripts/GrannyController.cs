using UnityEngine;

public class GrannyController : MonoBehaviour
{
    Granny_InputActions _inputActions;

    public float moveSpeed = 4f;
    public float jumpForce = 12f;
    public float turnSmoothing = .25f;

    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] Animator _anim;
    Vector2 moveInput;
    Vector2 aimInput;

    // Check for in the air
    [SerializeField] Collider[] col;
    public Transform groundCheck;
    public LayerMask thisIsGround;
    public bool isGrounded;

    // When in Shooter Mode
    public bool zoomedIn;
    public float rotationX;
    public float rotationY;
    public float playerRotationSpeed = 15f;
    public float playerLookSpeed = 50f;
    public Transform camTarget;
    public Unity.Cinemachine.InputAxis yAxis;

    private void Awake()
    {
        _inputActions = new Granny_InputActions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        _anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        col = Physics.OverlapSphere(groundCheck.position, .2f, thisIsGround);
        if (col.Length > 0) isGrounded = true;
        else isGrounded = false;

        moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        aimInput = _inputActions.Player.Camera.ReadValue<Vector2>();

        if (_inputActions.Player.Jump.triggered && isGrounded && !zoomedIn)
            PlayerJump();

        // Set Shooter Mode
        if (_inputActions.Player.Aim.IsPressed())
            zoomedIn = true;
        else zoomedIn = false;

        if (_inputActions.Player.Aim.triggered && isGrounded)
        {
            transform.rotation = Quaternion.Euler(0, cam.eulerAngles.y, 0);
        }

        // Animation States
        _anim.SetBool("Ground", isGrounded);
        _anim.SetFloat("VSpeed", rb.linearVelocity.y, .5f, Time.deltaTime);
        _anim.SetBool("Zoomed", zoomedIn);
        _anim.SetFloat("XDirection", moveInput.x, .1f, Time.deltaTime);
        _anim.SetFloat("YDirection", moveInput.y, .1f, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        _anim.SetFloat("MoveSpeed", moveDirection.magnitude, 0.075f, Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            if (moveDirection.magnitude >= .1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothing);

                if(!zoomedIn)
                    transform.rotation = Quaternion.Euler(0f, _angle, 0f);

                Vector3 moveDirCam = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }

        if (zoomedIn)
        {
            yAxis.Value = aimInput.y * playerLookSpeed * Time.deltaTime;
            rotationY += yAxis.Value;
            rotationY = Mathf.Clamp(rotationY, -55, 55);
            camTarget.localEulerAngles = new Vector3(-rotationY, 0f, 0f);

            rotationX = aimInput.x * playerRotationSpeed * Time.fixedDeltaTime;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y +  rotationX, 0); 
        }
    }

    void PlayerJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
