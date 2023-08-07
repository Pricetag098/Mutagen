using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAND : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        agent.agent.SetDestination(blackboard.moveToPosition);

        return child.Update();
    }
}
