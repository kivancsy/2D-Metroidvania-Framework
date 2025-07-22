using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    private int rollDirection;


    public override void Enter()
    {
        base.Enter();

        player.SetColliderSizeAndOffset(player.rollColliderSize, player.rollColliderOffset);

        rollDirection = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirection;

        stateTimer = player.rollDuration;
    }

    public override void Update()
    {
        base.Update();
        CancelRollIfNeeded();
        player.SetVelocity(player.rollSpeed * rollDirection, 0);


        if (stateTimer < 0)
        {
            if (player.groundDetected)
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

    private void CancelRollIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
}