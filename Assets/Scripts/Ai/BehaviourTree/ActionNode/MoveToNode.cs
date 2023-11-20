using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToNode : ActionNode
{
    [SerializeField] bool repeatTillReached;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (repeatTillReached)
        {
            if (Vector3.Distance(blackboard.moveToPosition, agent.transform.position) < 5)
                return State.Success;

            agent.agent.SetDestination(blackboard.moveToPosition);
            return State.Running;
        }
        else
        {
            agent.agent.SetDestination(blackboard.moveToPosition);
            return State.Success;
        }
    }
}
