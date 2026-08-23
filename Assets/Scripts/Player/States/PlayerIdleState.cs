using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.ResetGravityScale();

        Debug.Log("Entered Idle State");
    }

    public override void Update()
    {
        if (!player.IsGroundDetected() && player.Rigidbody.linearVelocity.y < -0.05f)
        {
            stateMachine.ChangeState(player.FallState);
            return;
        }

        if (player.Input.JumpPressed || player.CanUseBufferedJump)
        {
            player.ConsumeJumpBuffer();
            stateMachine.ChangeState(player.ChargeJumpState);
        }
    }
}