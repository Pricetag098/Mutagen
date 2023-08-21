using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUntilFailNode : ActionNode
{
    public float cutoffDistance;

    protected override void OnStart()
    {
        agent.agent.SetDestination(blackboard.moveToPosition);
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if(Vector3.Distance(agent.transform.position, agent.agent.destination) < cutoffDistance)
        {
            return State.Success;
        }
        return State.Running;



    }
}
