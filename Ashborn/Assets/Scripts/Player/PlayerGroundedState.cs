using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ResetAirDash();
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && player.groundDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        if (input.Player.Roll.WasPerformedThisFrame() && CanRoll())
            stateMachine.ChangeState(player.rollState);
    }

    private bool CanRoll()
    {
        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.rollState)
            return false;

        return true;
    }
}