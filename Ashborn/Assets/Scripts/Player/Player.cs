using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerRollState rollState { get; private set; }
    public PlayerAirDashState airDashState { get; private set; }
    public PlayerBasicAttackState basicAttackState { get; private set; }
    public PlayerJumpAttackState jumpattackState { get; private set; }
    public PlayerLedgeGrabState ledgeGrabState { get; private set; }

    [Header("Player Specific Collision Detection")]
    public Vector2 rollColliderSize = new Vector2(0.5f, 0.5f);

    public Vector2 rollColliderOffset = new Vector2(0f, 0.5f);
    [SerializeField] private float ledgeGrabDistance;
    [SerializeField] private Transform primaryLedgeGrabCheck;
    [SerializeField] private Transform secondaryLedgeGrabCheck;
    public bool isLedgeGrab { get; private set; }


    [Header("Attack Details")] public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    [Header("Movement Details")] public float jumpForce;
    [Range(0, 1)] public float inAirMoveMultiplier;
    public float rollDuration = .25f;
    public float rollSpeed = 20;
    public float airDashDuration = .25f;
    public float airDashSpeed = 20;
    public int maxAirDashes = 1;
    public int currentAirDashCount { get; private set; }
    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new PlayerIdleState(this, stateMachine, "isIdle");
        moveState = new PlayerMoveState(this, stateMachine, "isMove");
        jumpState = new PlayerJumpState(this, stateMachine, "isJump");
        fallState = new PlayerFallState(this, stateMachine, "isJump");
        rollState = new PlayerRollState(this, stateMachine, "isRoll");
        airDashState = new PlayerAirDashState(this, stateMachine, "isAirDash");
        basicAttackState = new PlayerBasicAttackState(this, stateMachine, "isBasicAttack");
        jumpattackState = new PlayerJumpAttackState(this, stateMachine, "isJumpAttack");
        ledgeGrabState = new PlayerLedgeGrabState(this, stateMachine, "isLedgeGrab");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        HandleLedgeGrabCollision();
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

    public void ResetAirDash()
    {
        currentAirDashCount = 0;
    }

    public void UseAirDash()
    {
        currentAirDashCount++;
    }

    public bool AirDashIsAvailable()
    {
        return currentAirDashCount < maxAirDashes;
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    private void HandleLedgeGrabCollision()
    {
        isLedgeGrab = Physics2D.Raycast(primaryLedgeGrabCheck.position, Vector2.right * facingDirection,
            ledgeGrabDistance,
            whatIsGround) && Physics2D.Raycast(secondaryLedgeGrabCheck.position, Vector2.right * facingDirection,
            ledgeGrabDistance,
            whatIsGround);
    }

    protected override void DrawGizmos()
    {
        base.DrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(primaryLedgeGrabCheck.position,
            primaryLedgeGrabCheck.position + new Vector3(ledgeGrabDistance * facingDirection, 0));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(secondaryLedgeGrabCheck.position,
            secondaryLedgeGrabCheck.position + new Vector3(ledgeGrabDistance * facingDirection, 0));
    }
}