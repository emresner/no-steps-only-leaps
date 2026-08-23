using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered Jump State");

        AudioManager.Instance?.PlayJump();

        player.ApplyAirMotionGravity();

        float horizontalVelocity = player.FacingDirection * player.CurrentHorizontalJumpForce * player.AirMotionSpeedMultiplier;
        float verticalVelocity = player.CurrentJumpForce * player.AirMotionSpeedMultiplier;

        player.Rigidbody.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    public override void Update()
    {
        if (player.Rigidbody.linearVelocity.y < 0f)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }
}