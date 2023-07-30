using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : SetterNode
{
    public enum TargetType
    {
        Player,
        Self,
        Danger,
    }

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
            case TargetType.Player:
                blackboard.targetPosition = agent.player.transform.position;
                break;
            case TargetType.Self:
                blackboard.targetPosition = agent.transform.position;
                break;

        }

        return child.Update();
    }
}
