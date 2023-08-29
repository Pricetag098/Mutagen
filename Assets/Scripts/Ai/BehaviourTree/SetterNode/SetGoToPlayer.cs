using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGoToPlayer : SetterNode
{

    protected override void OnStart()
    {

    }

    protected override void OnStop() 
    { 

    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition = agent.player.transform.position;

        return child.Update();
    }
}
