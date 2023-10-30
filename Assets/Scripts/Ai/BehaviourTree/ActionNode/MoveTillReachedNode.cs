using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTillReachedNode : ActionNode
{
    [SerializeField] bool repeatTillReached;
    protected override void OnStart()
    {
        Debug.Log("Start");
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (repeatTillReached)
        {
            Debug.Log("Running");
            if (Vector3.Distance(blackboard.moveToPosition, agent.transform.position) < 5)
                return State.Success;

            //Debug.Log("Running");
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
