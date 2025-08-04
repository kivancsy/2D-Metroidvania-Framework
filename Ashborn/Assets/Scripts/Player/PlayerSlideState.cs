using UnityEngine;

public class PlayerSlideState : PlayerGroundedState
{
    private int slideDirection;

    public PlayerSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.slideSpeed, rb.linearVelocity.y);
        player.SetColliderSizeAndOffset(player.slideColliderSize, player.slideColliderOffset);
        slideDirection = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirection;
        stateTimer = player.slideDuration;
    }

    public override void Update()
    {
        base.Update();
        CancelSlideIfNeeded();
        player.SetVelocity(player.slideSpeed * slideDirection, 0);

        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);

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

        player.ResetCollider();

        player.SetVelocity(0, 0);
    }

    private void CancelSlideIfNeeded()
    {
        if (player.isWallDetected)
            stateMachine.ChangeState(player.idleState);
    }
}