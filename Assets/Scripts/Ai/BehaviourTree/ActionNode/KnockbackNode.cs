using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackNode : ActionNode
{
    public float forceAmount;
    Vector3 awayDir;

    protected override void OnStart()
    {
        awayDir = new Vector3(agent.transform.position.x - blackboard.moveAwayObject.x,
                agent.transform.position.y, agent.transform.position.z - blackboard.moveAwayObject.z).normalized;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        agent.transform.position += awayDir * forceAmount;
        return State.Success;
    }
}
