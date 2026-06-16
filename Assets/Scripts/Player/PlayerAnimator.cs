using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator anim;
    public float acceleration = 5f;

    private static readonly int MoveHashX = Animator.StringToHash("MoveX"); 
    private static readonly int MoveHashY = Animator.StringToHash("MoveY");
    private static readonly int IdleRight = Animator.StringToHash("IdleRight");
    
    private static readonly int IdleLeft = Animator.StringToHash("IdleLeft");
    private static readonly int IdleFront = Animator.StringToHash("IdleFront");
    private static readonly int IdleBack = Animator.StringToHash("IdleBack");

    private Vector2 currentInput;
    private Vector2 lastValidInput = Vector2.down; 
    private bool isRunning;
    private float currentSpeedValueX;
    private float currentSpeedValueY;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove += UpdateMovementInput;
            InputManager.Instance.OnRun += UpdateRunInput;
        }
    }

    void Update()
    {
        
        float targetSpeedX = 0f;
        float targetSpeedY = 0f;

        if (currentInput.magnitude > 0.1f)
        {
            //Controllo se il player sta camminando o correndo
            float speedLimit = isRunning ? 1f : 0.5f;

            targetSpeedX = currentInput.x * speedLimit;
            targetSpeedY = currentInput.y * speedLimit;
            targetSpeedX = Mathf.Clamp(targetSpeedX, -1f, speedLimit);
            targetSpeedY = Mathf.Clamp(targetSpeedY, -1f, speedLimit);

            lastValidInput = currentInput;
        }

        currentSpeedValueX = Mathf.MoveTowards(currentSpeedValueX, targetSpeedX, acceleration * Time.deltaTime);
        currentSpeedValueY = Mathf.MoveTowards(currentSpeedValueY, targetSpeedY, acceleration * Time.deltaTime);

        // controllo se il player è fermo e cambio di posizione Idle
        if (currentInput == Vector2.zero && Mathf.Abs(currentSpeedValueX) < 0.05f && Mathf.Abs(currentSpeedValueY) < 0.05f)
        {
            MemoryDirection();

            anim.SetFloat(MoveHashX, 0f);
            anim.SetFloat(MoveHashY, 0f);
        }
        else
        {
            ResetAllIdleBools();
            anim.SetFloat(MoveHashX, currentSpeedValueX);
            anim.SetFloat(MoveHashY, currentSpeedValueY);
        }
    }

    void UpdateMovementInput(Vector2 input)
    {
        currentInput = input;
    }

    void UpdateRunInput(bool run)
    {
        isRunning = run;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= UpdateMovementInput;
            InputManager.Instance.OnRun -= UpdateRunInput;
        }
    }

    public void MemoryDirection()
    {
        ResetAllIdleBools();

        if (Mathf.Abs(lastValidInput.x) > Mathf.Abs(lastValidInput.y))
        {
            // Posizione ultima idle salvata Orizzontale
            if (lastValidInput.x > 0f)
                anim.SetBool(IdleRight, true);
            else
                anim.SetBool(IdleLeft, true);
        }
        else
        {
            // Posizione ultima idle salvata Verticale
            if (lastValidInput.y > 0f)
                anim.SetBool(IdleBack, true); 
            else
                anim.SetBool(IdleFront, true); 
        }
    }

    // Reset delle posizione di Idle prima di memorizzare l'ultima direzione 
    private void ResetAllIdleBools()
    {
        anim.SetBool("IdleLeft", false);
        anim.SetBool("IdleRight", false);
        anim.SetBool("IdleFront", false);
        anim.SetBool("IdleBack", false);
    }
}
