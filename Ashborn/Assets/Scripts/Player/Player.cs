using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }

    [Header("Movement Details")] public float jumpForce;
    [Range(0, 1)] public float inAirMoveMultiplier;

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        input = new PlayerInputSet();

        idleState = new PlayerIdleState(this, stateMachine, "isIdle");
        moveState = new PlayerMoveState(this, stateMachine, "isMove");
        jumpState = new PlayerJumpState(this, stateMachine, "isJump");
        fallState = new PlayerFallState(this, stateMachine, "isJump");

    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }
}