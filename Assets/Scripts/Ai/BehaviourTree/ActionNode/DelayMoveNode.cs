using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayMoveNode : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        agent.delayMove = true;
        agent.delayMoveTimer = Time.time;
        return State.Success;
    }
}
