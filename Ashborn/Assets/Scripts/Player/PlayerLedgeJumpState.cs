using UnityEngine;

public class PlayerLedgeJumpState : PlayerAiredState
{
    public PlayerLedgeJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetColliderSizeAndOffset(player.ledgeJumpColliderSize, player.ledgeJumpColliderOffset);
        player.SetVelocity(player.ledgeJumpForce.x * -player.facingDirection, player.ledgeJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }

    public override void Exit()
    {
        base.Exit();
        player.ResetCollider();
    }
}