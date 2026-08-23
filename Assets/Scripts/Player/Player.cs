using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Animator animator;

    [Header("Jump Settings")]
    [SerializeField] private float minJumpForce = 1.2f;
    [SerializeField] private float maxJumpForce = 12f;
    [SerializeField] private float maxChargeTime = 0.6f;
    [SerializeField] private float maxHorizontalJumpForce = 5f;
    [SerializeField] private float minHorizontalJumpForce = 1.5f;
    [SerializeField] private float airMotionSpeedMultiplier = 1.2f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.8f;
    [SerializeField] private float groundCheckWidth = 0.35f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Bounce Settings")]
    [SerializeField] private float minWallBounceForce = 3.2f;
    [SerializeField] private float maxWallBounceForce = 8f;
    [SerializeField] private float wallBounceVelocityMultiplier = 1.25f;
    [SerializeField] private float wallBounceCooldown = 0.08f;

    [Header("Charge Visual Settings")]
    [SerializeField] private float chargeSquashHeight = 1.6f;

    [Header("Hard Landing Settings")]
    [SerializeField] private float hardLandingHeightThreshold = 14f;
    [SerializeField] private float hitStateDuration = 0.85f;

    [Header("Hit Recovery Settings")]
    [SerializeField] private float postHitInvulnerabilityDuration = 0.35f;

    public Rigidbody2D Rigidbody { get; private set; }
    public InputManager Input => inputManager;

    public StateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerChargeJumpState ChargeJumpState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerHitState HitState { get; private set; }

    public int FacingDirection { get; private set; } = 1;
    public float CurrentJumpForce { get; private set; }
    public float CurrentHorizontalJumpForce { get; private set; }
    public bool CanControl { get; private set; } = true;
    public Vector2 HitVelocity { get; private set; }

    public float MinJumpForce => minJumpForce;
    public float MaxJumpForce => maxJumpForce;
    public float MaxChargeTime => maxChargeTime;
    public float HorizontalJumpForce => maxHorizontalJumpForce;
    public float AirMotionSpeedMultiplier => airMotionSpeedMultiplier;
    public float MinHorizontalJumpForce => minHorizontalJumpForce;
    public float ChargeSquashHeight => chargeSquashHeight;
    public float HardLandingHeightThreshold => hardLandingHeightThreshold;
    public float HitStateDuration => hitStateDuration;
    public bool CanTakeTrapHit => Time.time >= trapHitInvulnerabilityEndTime;


    public bool HasBufferedJump => jumpBufferTimer > 0f;
    public bool CanUseBufferedJump => HasBufferedJump && inputManager.JumpHeld;

    public Transform VisualTransform => visualTransform;

    public Vector3 DefaultVisualScale { get; private set; }
    public Vector3 DefaultVisualLocalPosition { get; private set; }

    private float jumpBufferTimer;
    private float lastWallBounceTime = -Mathf.Infinity;
    private float defaultGravityScale;
    private float trapHitInvulnerabilityEndTime = -Mathf.Infinity;
    private bool isEndingWalking;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        defaultGravityScale = Rigidbody.gravityScale;

        CacheDefaultVisualTransform();
        InitializeStateMachine();
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        if (CanControl)
        {
            UpdateJumpBuffer();
            HandleFacingDirection();
            StateMachine.Update();
        }
        else
        {
            ClearJumpBuffer();
        }

        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void UpdateJumpBuffer()
    {
        if (inputManager.JumpPressed)
        {
            jumpBufferTimer = jumpBufferTime;
            return;
        }

        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    public void ConsumeJumpBuffer()
    {
        jumpBufferTimer = 0f;
    }

    public void ClearJumpBuffer()
    {
        jumpBufferTimer = 0f;
    }

    private void CacheDefaultVisualTransform()
    {
        DefaultVisualScale = visualTransform.localScale;
        DefaultVisualLocalPosition = visualTransform.localPosition;
    }

    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        ChargeJumpState = new PlayerChargeJumpState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);
        HitState = new PlayerHitState(this, StateMachine);
    }

    private void HandleFacingDirection()
    {
        if (!CanChangeFacingDirection())
            return;

        if (inputManager.LookInput == 0f)
            return;

        SetFacingDirection(inputManager.LookInput > 0f ? 1 : -1);
    }

    private bool CanChangeFacingDirection()
    {
        return StateMachine.CurrentState == IdleState ||
               StateMachine.CurrentState == ChargeJumpState;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isGrounded", IsGroundDetected());
        animator.SetFloat("yVelocity", Rigidbody.linearVelocity.y);
        animator.SetBool("isHit", StateMachine.CurrentState == HitState);
        animator.SetBool("isEndingWalking", isEndingWalking);
    }

    public void SetJumpForces(float verticalJumpForce, float horizontalJumpForce)
    {
        CurrentJumpForce = verticalJumpForce;
        CurrentHorizontalJumpForce = horizontalJumpForce;
    }

    public void ApplyAirMotionGravity()
    {
        Rigidbody.gravityScale = defaultGravityScale * airMotionSpeedMultiplier * airMotionSpeedMultiplier;
    }

    public void ResetGravityScale()
    {
        Rigidbody.gravityScale = defaultGravityScale;
    }

    public bool IsGroundDetected()
    {
        Vector2 centerOrigin = transform.position;
        Vector2 leftOrigin = centerOrigin + Vector2.left * groundCheckWidth;
        Vector2 rightOrigin = centerOrigin + Vector2.right * groundCheckWidth;

        bool centerHit = Physics2D.Raycast(
            centerOrigin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        bool leftHit = Physics2D.Raycast(
            leftOrigin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        bool rightHit = Physics2D.Raycast(
            rightOrigin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return centerHit || leftHit || rightHit;
    }

    public void ResetVisualTransform()
    {
        Vector3 scale = DefaultVisualScale;
        scale.x = Mathf.Abs(DefaultVisualScale.x) * FacingDirection;

        visualTransform.localScale = scale;
        visualTransform.localPosition = DefaultVisualLocalPosition;
    }

    public void SetVisualVisible(bool visible)
    {
        visualTransform.gameObject.SetActive(visible);
    }

    public void SetEndingWalking(bool isWalking)
    {
        isEndingWalking = isWalking;
    }

    public void SetFacingDirection(int direction)
    {
        if (direction == 0)
            return;

        FacingDirection = direction > 0 ? 1 : -1;

        Vector3 scale = visualTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * FacingDirection;
        visualTransform.localScale = scale;
    }

    public void TakeTrapHit(Vector2 hitVelocity)
    {
        if (StateMachine.CurrentState == HitState)
            return;

        if (!CanTakeTrapHit)
            return;

        ClearJumpBuffer();
        ResetVisualTransform();

        AudioManager.Instance?.PlayTrapHit();
        CameraShakeManager.Instance?.ShakeTrapHit();

        HitVelocity = hitVelocity;

        if (Mathf.Abs(hitVelocity.x) > 0.01f)
        {
            SetFacingDirection(hitVelocity.x > 0f ? 1 : -1);
        }

        StateMachine.ChangeState(HitState);
    }

    public void TakeHardLandingHit()
    {
        ClearJumpBuffer();

        AudioManager.Instance?.PlayTrapHit();
        CameraShakeManager.Instance?.ShakeHardLanding();

        HitVelocity = Vector2.zero;

        StateMachine.ChangeState(HitState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryWallBounce(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryWallBounce(collision);
    }

    private void TryWallBounce(Collision2D collision)
    {
        if (!CanWallBounce())
            return;

        if (Time.time < lastWallBounceTime + wallBounceCooldown)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            bool hitWall = Mathf.Abs(normal.x) > 0.5f;

            if (!hitWall)
                continue;

            float bounceDirection = normal.x > 0f ? 1f : -1f;
            float dynamicBounceForce = CalculateWallBounceForce(collision);

            SetFacingDirection((int)bounceDirection);

            Rigidbody.linearVelocity = new Vector2(
                bounceDirection * dynamicBounceForce,
                Rigidbody.linearVelocity.y
            );

            AudioManager.Instance?.PlayWallBounce();
            
            lastWallBounceTime = Time.time;
            return;
        }
    }

    private float CalculateWallBounceForce(Collision2D collision)
    {
        float impactHorizontalSpeed = Mathf.Abs(collision.relativeVelocity.x);

        return Mathf.Clamp(
            impactHorizontalSpeed * wallBounceVelocityMultiplier,
            minWallBounceForce,
            maxWallBounceForce
        );
    }

    private bool CanWallBounce()
    {
        return StateMachine.CurrentState == JumpState ||
               StateMachine.CurrentState == FallState;
    }

    public void StartPostHitInvulnerability()
    {
        trapHitInvulnerabilityEndTime = Time.time + postHitInvulnerabilityDuration;
    }

    public void EnterEndingMode()
    {
        ClearJumpBuffer();
        ResetVisualTransform();
        ResetGravityScale();
        SetEndingWalking(false);

        Rigidbody.linearVelocity = Vector2.zero;

        if (StateMachine != null && StateMachine.CurrentState != IdleState)
        {
            StateMachine.ChangeState(IdleState);
        }

        SetControlEnabled(false);
    }

    public void SetControlEnabled(bool enabled)
    {
        CanControl = enabled;

        if (!enabled)
        {
            Rigidbody.linearVelocity = Vector2.zero;
            ClearJumpBuffer();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 centerOrigin = transform.position;
        Vector3 leftOrigin = centerOrigin + Vector3.left * groundCheckWidth;
        Vector3 rightOrigin = centerOrigin + Vector3.right * groundCheckWidth;

        Gizmos.DrawLine(
            centerOrigin,
            centerOrigin + Vector3.down * groundCheckDistance
        );

        Gizmos.DrawLine(
            leftOrigin,
            leftOrigin + Vector3.down * groundCheckDistance
        );

        Gizmos.DrawLine(
            rightOrigin,
            rightOrigin + Vector3.down * groundCheckDistance
        );
    }
}