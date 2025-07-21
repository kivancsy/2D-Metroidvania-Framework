using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public PlayerIdleState idleState { get; private set; }

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new PlayerIdleState(this, stateMachine, "isIdle");

        input = new PlayerInputSet();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
}