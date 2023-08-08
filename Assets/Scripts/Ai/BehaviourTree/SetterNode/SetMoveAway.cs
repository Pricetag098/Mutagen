using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAwayType
{
    player,
    danger,
};

public class SetMoveAway : SetterNode
{
    public MoveAwayType moveAwayType;
    public float distance;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        switch (moveAwayType)
        {
            case MoveAwayType.player:
                blackboard.moveToPosition = new Vector3(agent.transform.position.x - agent.player.transform.position.x,
                    agent.transform.position.y, agent.transform.position.z - agent.player.transform.position.z).normalized * distance; 
                break;
            case MoveAwayType.danger:
                if (agent.dangerObject != null)
                    blackboard.moveToPosition = agent.dangerObject.transform.position;
                break;

        }

        return child.Update();
    }
}
