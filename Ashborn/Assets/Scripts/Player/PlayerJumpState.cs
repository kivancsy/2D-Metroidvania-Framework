using UnityEngine;

public class PlayerJumpState : PlayerAiredState
{
    public PlayerJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
        Debug.Log(rb.linearVelocity.x);
    }

    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }
}