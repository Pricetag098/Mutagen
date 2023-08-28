using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetRotateTowards : SetterNode
{
    public TargetType target;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        switch (target)
        {
            case TargetType.player:
                blackboard.rotateTowardsObject = player.gameObject;
                break;

        }
        return child.Update();
    }
}
