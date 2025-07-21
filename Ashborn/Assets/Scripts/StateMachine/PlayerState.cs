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
    }

    public override void UpdateAnimatonParameters()
    {
        base.UpdateAnimatonParameters();
    }

    private bool CanDash()
    {
        return true;
    }
}