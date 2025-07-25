using UnityEngine;

public class PlayerLedgeGrabState : PlayerState
{
    public PlayerLedgeGrabState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
        player.ResetAirDash();
        player.DisableGravity();
    }

    public override void Update()
    {
        base.Update();
        HandleLedgeGrab();

        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.ledgeJumpState);
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

        if (player.moveInput.y == -1)
        {
            stateMachine.ChangeState(player.fallState);
            player.DoLedgeDropPushBack();
            return;
        }

        // if (player.moveInput.x != 0 && player.moveInput.x != player.facingDirection)
        // {
        //     stateMachine.ChangeState(player.fallState);
        //     return;
        // }

        player.SetVelocity(0, 0);
    }
}