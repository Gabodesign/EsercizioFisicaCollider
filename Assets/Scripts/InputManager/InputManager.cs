using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public PlayerControls controls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;   
        DontDestroyOnLoad(gameObject);

        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        controls.Player.Run.performed += ctx => OnRun?.Invoke(true);
        controls.Player.Run.canceled += ctx => OnRun?.Invoke(false);

        controls.Player.Jump.performed += ctx => OnJump?.Invoke();


        controls.Player.Interact.performed += ctx => OnInteract?.Invoke();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Disable();
        }
    }

    public event System.Action<Vector2> OnMove;
    public event System.Action<bool> OnRun;
    public event System.Action OnJump;
    public event System.Action OnInteract;
}
