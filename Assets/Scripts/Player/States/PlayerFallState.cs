using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float fallStartY;

    public PlayerFallState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.ApplyAirMotionGravity();

        fallStartY = player.transform.position.y;
    }

    public override void Update()
    {
        if (!player.IsGroundDetected())
            return;

        player.Rigidbody.linearVelocity = Vector2.zero;

        float landingY = player.transform.position.y;
        float fallDistance = fallStartY - landingY;

        if (fallDistance >= player.HardLandingHeightThreshold)
        {
            player.TakeHardLandingHit();
            return;
        }

        if (player.CanUseBufferedJump)
        {
            player.ConsumeJumpBuffer();
            stateMachine.ChangeState(player.ChargeJumpState);
            return;
        }

        stateMachine.ChangeState(player.IdleState);
    }
}