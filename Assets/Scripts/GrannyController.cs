using UnityEngine;

public class GrannyController : MonoBehaviour
{
    Granny_InputActions _inputActions;

    public float moveSpeed = 4f;
    public float jumpForce = 12f;

    [SerializeField] Rigidbody rb;
    Vector2 moveInput;

    private void Awake()
    {
        _inputActions = new Granny_InputActions();
    }
 
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
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

        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
