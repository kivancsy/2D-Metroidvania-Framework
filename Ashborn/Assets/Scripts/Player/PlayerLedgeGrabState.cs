using UnityEngine;

public class PlayerLedgeGrabState : PlayerAiredState
{
    public PlayerLedgeGrabState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
        player.DisableGravity();
    }

    public override void Update()
    {
        base.Update();
        HandleLedgeGrab();
    }

    public override void Exit()
    {
        base.Exit();
        player.EnableGravity();
    }

    private void HandleLedgeGrab()
    {
        if (!player.isLedgeGrab)
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }

        if (player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);

            return;
        }

        if (Mathf.Approximately(player.moveInput.y, -player.facingDirection))
        {
            player.Flip();
            stateMachine.ChangeState(player.fallState);
            return;
        }

        player.SetVelocity(0, 0);
    }
}