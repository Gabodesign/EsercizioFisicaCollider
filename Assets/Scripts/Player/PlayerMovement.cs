using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settaglio Movimento, salto e rotazione")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] public GameObject target;

    private Rigidbody rb;
    private Vector2 direction;
    private bool isRunning;
    private bool isGrounded;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove += HandleMoveInput;
            InputManager.Instance.OnRun += HandleRunToggle;
            InputManager.Instance.OnJump += HandleJumpInput;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= HandleMoveInput;
            InputManager.Instance.OnRun -= HandleRunToggle;
            InputManager.Instance.OnJump -= HandleJumpInput;
        }
    }

    void Start()
    {
        if (InputManager.Instance != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            ExecuteJump();
        }
    }

    public void HandleMoveInput(Vector2 input) => direction = input;
    public void HandleRunToggle(bool runState) => isRunning = runState;
    public void HandleJumpInput()
    {
        if (isGrounded)
        {
            jumpRequested = true;
        }
    }
    public bool HasMovementInput() => direction != Vector2.zero;
    public bool IsRunningInput() => isRunning;

    public void ApplyMove()
    {
        Debug.Log("direction: " + direction.x + ", direction2: " + direction.y);
        if (direction == Vector2.zero)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);

        Vector3 targetVelocity = moveDirection * targetSpeed;

 
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

        ApplyRotation(moveDirection);
    }

    public void ApplyRotation(Vector3 moveDirection)
    {
        if (moveDirection.magnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void ExecuteJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpRequested = false; // Reset della richiesta
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}