using UnityEngine;

public class PlayerAiredState : PlayerState
{
    public PlayerAiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier),
                rb.linearVelocity.y);

        if (input.Player.AirDash.WasPressedThisFrame() && CanAirDash() && player.AirDashIsAvailable())
        {
            stateMachine.ChangeState(player.airDashState);
        }
    }

    private bool CanAirDash()
    {
        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.airDashState)
            return false;

        return true;
    }
}