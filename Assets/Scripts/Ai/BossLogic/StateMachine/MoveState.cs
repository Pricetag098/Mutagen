using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{


    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void Tick()
    {
        manager.nav.SetDestination(manager.movementTarget.position);
    }
}
