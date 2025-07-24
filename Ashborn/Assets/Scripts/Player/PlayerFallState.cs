using UnityEngine;

public class PlayerFallState : PlayerAiredState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.IncreaseGravity();
    }

    public override void Update()
    {
        base.Update();

        if (player.isGroundDetected)
            stateMachine.ChangeState(player.idleState);

        if (player.CanGrabLedge())
            stateMachine.ChangeState(player.ledgeGrabState);
    }
}