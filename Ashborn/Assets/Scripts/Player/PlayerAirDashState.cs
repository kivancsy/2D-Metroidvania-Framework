using UnityEngine;

public class PlayerAirDashState : PlayerAiredState
{
    private int dashDirection;

    public PlayerAirDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.UseAirDash();

        dashDirection = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirection;
        stateTimer = player.airDashDuration;

        player.DisableGravity();
    }

    public override void Update()
    {
        base.Update();
        CancelAirDashIfNeeded();
        player.SetVelocity(player.airDashSpeed * dashDirection, 0);

        if (stateTimer < 0)
        {
            if (player.isGroundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.EnableGravity();
        player.SetVelocity(0, 0);
    }

    private void CancelAirDashIfNeeded()
    {
        if (player.isWallDetected)
        {
            if (player.isGroundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
}