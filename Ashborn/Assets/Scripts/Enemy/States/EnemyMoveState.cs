using UnityEngine;

public class EnemyMoveState : EnemyGroundedState
{
    public EnemyMoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.isGroundDetected == false || enemy.isWallDetected)
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.linearVelocity.y);

        if (enemy.isGroundDetected == false || enemy.isWallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }
}