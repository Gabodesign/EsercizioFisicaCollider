using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerMovement playerMovement;

    private Rigidbody rb;
    private void Awake()
    {
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        playerMovement.ApplyMove();
    }


    public bool HasMovementInput() => playerMovement != null && playerMovement.HasMovementInput();
    public bool IsRunningInput() => playerMovement != null && playerMovement.IsRunningInput();
}