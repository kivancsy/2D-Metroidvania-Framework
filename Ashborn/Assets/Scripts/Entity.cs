using UnityEngine;
using UnityEngine.XR;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D capsule { get; private set; }
    public StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;

    [Header("Movement Details")] public float moveSpeed;

    [Header("Collision detection")] [SerializeField]
    protected LayerMask whatIsGround;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public Vector2 originalColliderSize { get; private set; }
    public Vector2 originalColliderOffset { get; private set; }
    public float defaultGravityScale { get; private set; }
    public bool isWallDetected { get; private set; }
    public bool isGroundDetected { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();

        defaultGravityScale = rb.gravityScale;
        originalColliderSize = capsule.size;
        originalColliderOffset = capsule.offset;

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirection = facingDirection * -1;
    }

    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = defaultGravityScale;
    }

    public void IncreaseGravity()
    {
        rb.gravityScale += rb.gravityScale;
    }

    public void SetColliderSizeAndOffset(Vector2 colliderSize, Vector2 colliderOffset)
    {
        if (capsule == null) return;
        capsule.size = colliderSize;
        capsule.offset = colliderOffset;
    }

    public void ResetCollider()
    {
        SetColliderSizeAndOffset(originalColliderSize, originalColliderOffset);
    }

    private void HandleCollisionDetection()
    {
        isGroundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);


        if (secondaryWallCheck != null)
        {
            isWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection,
                                 wallCheckDistance,
                                 whatIsGround)
                             && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDirection,
                                 wallCheckDistance, whatIsGround);
        }
        else
            isWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection,
                wallCheckDistance,
                whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        DrawGizmos();
    }

    protected virtual void DrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position,
            primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));

        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position,
                secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}