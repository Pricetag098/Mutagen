using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToNode : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        agent.isMoving = true;
        agent.agent.SetDestination(blackboard.moveToPosition);
        return State.Success;
    }
}
