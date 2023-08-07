using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateTarget
{
    player
}

public class SetRotateTowards : SetterNode
{
    public RotateTarget target;

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
            case RotateTarget.player:
                blackboard.rotateTowardsObject = agent.player.gameObject;
                return child.Update();

        }
        return child.Update();
    }
}
