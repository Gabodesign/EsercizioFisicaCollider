using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator anim;

    private Vector2 currentInput;
    private Vector2 lastValidInput = Vector2.down; 
    private bool isRunning;
    private float currentSpeedValueX;
    private float currentSpeedValueY;
    public float acceleration = 5f;
    

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
            float speedLimit = isRunning ? 1f : 0.5f;

            targetSpeedX = currentInput.x * speedLimit;
            targetSpeedY = currentInput.y * speedLimit;
            targetSpeedX = Mathf.Clamp(targetSpeedX, -1f, speedLimit);
            targetSpeedY = Mathf.Clamp(targetSpeedY, -1f, speedLimit);

            lastValidInput = currentInput;
        }

        currentSpeedValueX = Mathf.MoveTowards(currentSpeedValueX, targetSpeedX, acceleration * Time.deltaTime);
        currentSpeedValueY = Mathf.MoveTowards(currentSpeedValueY, targetSpeedY, acceleration * Time.deltaTime);

        if (currentInput == Vector2.zero && Mathf.Abs(currentSpeedValueX) < 0.05f && Mathf.Abs(currentSpeedValueY) < 0.05f)
        {
            MemoryDirection();

            anim.SetFloat("MoveX", 0f);
            anim.SetFloat("MoveY", 0f);
        }
        else
        {
            ResetAllIdleBools();
            anim.SetFloat("MoveX", currentSpeedValueX);
            anim.SetFloat("MoveY", currentSpeedValueY);
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
            // Movimento Orizzontale
            if (lastValidInput.x > 0f)
                anim.SetBool("IdleRight", true);
            else
                anim.SetBool("IdleLeft", true);
        }
        else
        {
            // Movimento Verticale
            if (lastValidInput.y > 0f)
                anim.SetBool("IdleBack", true); 
            else
                anim.SetBool("IdleFront", true); 
        }
    }

    
    private void ResetAllIdleBools()
    {
        anim.SetBool("IdleLeft", false);
        anim.SetBool("IdleRight", false);
        anim.SetBool("IdleFront", false);
        anim.SetBool("IdleBack", false);
    }
}