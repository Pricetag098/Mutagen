using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNode : DecoratorNode
{
    public int eventIndex;
    Event action;

    protected override void OnStart()
    {
        action = agent.eventManager.events[eventIndex];
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (!action.used)
        {
            action.Play(agent);
        }
        return child.Update();
    }
}
