using System;
using UnityEngine;

public class Player : Entity
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //stateMachine.Initialize();
    }
}