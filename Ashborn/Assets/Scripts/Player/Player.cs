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
    public PlayerLedgeJumpState ledgeJumpState { get; private set; }
    public PlayerSlideState slideState { get; private set; }

    [Header("Player Specific Collision Detection")]
    public Vector2 rollColliderSize = new Vector2(0.5f, 0.5f);

    public Vector2 rollColliderOffset = new Vector2(0f, 0.5f);
    public Vector2 ledgeJumpColliderSize = new Vector2(0.5f, 0.5f);
    public Vector2 ledgeJumpColliderOffset = new Vector2(0.5f, 0.5f);
    public Vector2 slideColliderSize = new Vector2(0.5f, 0.5f);
    public Vector2 slideColliderOffset = new Vector2(0f, 0.5f);
    [SerializeField] private float ledgeGrabDistance;
    [SerializeField] private float ledgeClearDistance;
    [SerializeField] private Transform ledgeGrabFrontCheck;
    [SerializeField] private Transform ledgeGrabAboveCheck;
    [SerializeField] private Transform ledgeClearCheck;
    public bool isLedgeGrab { get; private set; }
    public bool isLedgeGrabAboveClear { get; private set; }
    public bool isLedgeClear { get; private set; }


    [Header("Player Attack Details")] public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    [Header("Player Movement Details")] public float jumpForce;
    [Range(0, 1)] public float inAirMoveMultiplier;
    public float rollDuration = .25f;
    public float rollSpeed = 20;
    public float airDashDuration = .25f;
    public float airDashSpeed = 20;
    public int maxAirDashes = 1;
    public float ledgeDropPushBackAmount = 0.5f;
    public Vector2 ledgeJumpForce;
    public float slideDuration = .25f;
    public float slideSpeed = 12;
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
        ledgeJumpState = new PlayerLedgeJumpState(this, stateMachine, "isLedgeJump");
        slideState = new PlayerSlideState(this, stateMachine, "isSlide");
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

    public void DoLedgeDropPushBack()
    {
        Vector2 pushBackDir = new Vector2(-facingDirection, 0);
        transform.position += (Vector3)(pushBackDir * ledgeDropPushBackAmount);
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
        isLedgeGrab =
            Physics2D.Raycast(ledgeGrabFrontCheck.position, Vector2.right * facingDirection,
                ledgeGrabDistance, whatIsGround);

        isLedgeGrabAboveClear = !Physics2D.Raycast(ledgeGrabAboveCheck.position, Vector2.right * facingDirection,
            ledgeGrabDistance, whatIsGround);

        isLedgeClear = !Physics2D.Raycast(ledgeClearCheck.position, Vector2.up,
            ledgeClearDistance,
            whatIsGround);
    }

    public bool CanGrabLedge()
    {
        return isLedgeGrab && isLedgeGrabAboveClear && isLedgeClear;
    }

    protected override void DrawGizmos()
    {
        base.DrawGizmos();
        if (ledgeGrabAboveCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(ledgeGrabFrontCheck.position,
                ledgeGrabFrontCheck.position + new Vector3(ledgeGrabDistance * facingDirection, 0));
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(ledgeGrabAboveCheck.position,
                ledgeGrabAboveCheck.position + new Vector3(ledgeGrabDistance * facingDirection, 0));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(ledgeClearCheck.position,
                ledgeClearCheck.position + new Vector3(0, ledgeClearDistance));
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(ledgeGrabFrontCheck.position,
                ledgeGrabFrontCheck.position + new Vector3(ledgeGrabDistance * facingDirection, 0));
        }
    }
}