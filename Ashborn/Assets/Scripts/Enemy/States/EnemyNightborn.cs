using UnityEngine;

public class EnemyNightborn : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(this, stateMachine, "isIdle");
        moveState = new EnemyMoveState(this, stateMachine, "isMove");
        attackState = new EnemyAttackState(this, stateMachine, "isAttack");
        battleState = new EnemyBattleState(this, stateMachine, "isBattle");
        deathState = new EnemyDeathState(this, stateMachine, "isDead");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
}