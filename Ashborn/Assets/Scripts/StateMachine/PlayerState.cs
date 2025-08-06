using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;

    protected PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine,
        animBoolName)
    {
        this.player = player;

        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Roll.WasPerformedThisFrame() && CanRoll())
            stateMachine.ChangeState(player.rollState);
    }


    private bool CanRoll()
    {
        return !player.isWallDetected && !IsInRestrictedState();
    }

    private bool IsInRestrictedState()
    {
        return stateMachine.currentState == player.rollState
               || stateMachine.currentState == player.jumpState
               || stateMachine.currentState == player.fallState
               || stateMachine.currentState == player.jumpattackState
               || stateMachine.currentState == player.slideState
               || stateMachine.currentState == player.ledgeGrabState;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }
}