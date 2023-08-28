using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : SetterNode
{
    public TargetType targetType;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        switch (targetType)
        {
            case TargetType.player:
                blackboard.targetPosition = player.transform.position;
                break;
            case TargetType.self:
                blackboard.targetPosition = agent.transform.position;
                break;

        }

        return child.Update();
    }
}
