using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        agent.transform.LookAt(blackboard.rotateTowardsObject.transform);
        return child.Update();
    }
}
