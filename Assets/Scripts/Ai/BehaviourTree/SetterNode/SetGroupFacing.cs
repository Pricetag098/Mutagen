using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGroupFacing : SetterNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition = groupFacing();
        return child.Update();
    }

    Vector3 groupFacing()
    {
        Vector3 playerFlank = player.transform.position + 
            ((-player.transform.forward * agent.circlingDistance).normalized);
        return playerFlank;
    }
}
