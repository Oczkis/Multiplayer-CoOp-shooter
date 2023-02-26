using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [Header("References")]
    public Transform model;
    public Rigidbody rb;
    public PlayerInputActions inputActions;
    public Vector2 movementInput;

    [Header("Settings")]
    public float speed = 2;
    public float turnSpeed = 90;

    private PlayerCharacterMotor characterMotor;
    private PlayerGun playerGun;
    private PlayerCharacter playerCharacter;

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        rb = GetComponent<Rigidbody>();
        characterMotor = GetComponent<PlayerCharacterMotor>();
        playerGun = GetComponent<PlayerGun>();
    }

    public override void OnStartAuthority()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerActions.ToggleScoreBoard.started += inputActions => ScoreBoardManager.Instance.ToggleScoreBoard();
            inputActions.PlayerActions.Fire.started += inputActions => playerGun.Aim(true);
            inputActions.PlayerActions.Fire.canceled += inputActions => playerGun.Aim(false);

            inputActions.Enable();
        }
    }

    public override void OnStopAuthority()
    {
        inputActions.Disable();
    }

    void Update()
    { 
        if(!hasAuthority || !playerCharacter.isAlive) { return; }
        characterMotor.TickInput(movementInput);
    }

    private void FixedUpdate()
    {
        if (!hasAuthority) { return; }
        characterMotor.Move();
    }
}
