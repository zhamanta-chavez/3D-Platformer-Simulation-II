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
    Vector2 moveInput;

    private void Awake()
    {
        _inputActions = new Granny_InputActions();
    }
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
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
        moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (moveDirection != Vector3.zero)
        {
            if (moveDirection.magnitude >= .1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothing);
                transform.rotation = Quaternion.Euler(0f, _angle, 0f);

                Vector3 moveDirCam = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
