using System.Collections;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.SwitchOffStateMachine();

        enemy.StartCoroutine(DeathDelay());
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(enemy.deathAnimationDuration);
        GameObject.Destroy(enemy.gameObject);
    }
}