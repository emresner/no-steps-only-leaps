using UnityEngine;

public class PlayerHitState : PlayerState
{
    private float hitTimer;

    public PlayerHitState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        hitTimer = 0f;

        player.Rigidbody.linearVelocity = player.HitVelocity;
    }

    public override void Update()
    {
        hitTimer += Time.deltaTime;

        if (hitTimer < player.HitStateDuration)
            return;

        player.StartPostHitInvulnerability();

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        stateMachine.ChangeState(player.FallState);
    }
}