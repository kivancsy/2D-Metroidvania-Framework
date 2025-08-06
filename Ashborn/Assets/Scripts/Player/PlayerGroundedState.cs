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
        player.EnableGravity();
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && player.isGroundDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        if (input.Player.Attack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);

        if (input.Player.Slide.WasPerformedThisFrame() && CanSlide())
            stateMachine.ChangeState(player.slideState);
    }
    

    private bool CanSlide()
    {
        if (player.isWallDetected)
            return false;

        if (stateMachine.currentState == player.slideState)
            return false;

        if (stateMachine.currentState != player.moveState)
            return false;

        return true;
    }
}