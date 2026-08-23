using UnityEngine;

public class PlayerChargeJumpState : PlayerState
{
    private float chargeTimer;
    private int chargeDirection;

    public PlayerChargeJumpState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        chargeTimer = 0f;
        chargeDirection = player.FacingDirection;

        player.ResetVisualTransform();

        Debug.Log("Entered Charge Jump State");
    }

    public override void Update()
    {
        if (player.FacingDirection != chargeDirection)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        chargeTimer += Time.deltaTime;
        chargeTimer = Mathf.Clamp(chargeTimer, 0f, player.MaxChargeTime);

        float chargePercent = chargeTimer / player.MaxChargeTime;

        float xScale = Mathf.Lerp(1f, 1.2f, chargePercent);
        float yScale = Mathf.Lerp(1f, 0.8f, chargePercent);

        Vector3 scale = player.DefaultVisualScale;
        scale.x = Mathf.Abs(player.DefaultVisualScale.x) * player.FacingDirection * xScale;
        scale.y = yScale;

        player.VisualTransform.localScale = scale;

        float heightLoss = player.ChargeSquashHeight * (1f - yScale);

        Vector3 position = player.DefaultVisualLocalPosition;
        position.y = player.DefaultVisualLocalPosition.y - heightLoss * 0.5f;

        player.VisualTransform.localPosition = position;

        if (player.Input.JumpReleased)
        {
            float verticalJumpForce = Mathf.Lerp(
                player.MinJumpForce,
                player.MaxJumpForce,
                chargePercent
            );

            float horizontalJumpForce = Mathf.Lerp(
                player.MinHorizontalJumpForce,
                player.HorizontalJumpForce,
                chargePercent
            );

            player.SetJumpForces(verticalJumpForce, horizontalJumpForce);

            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void Exit()
    {
        player.ResetVisualTransform();
    }
}