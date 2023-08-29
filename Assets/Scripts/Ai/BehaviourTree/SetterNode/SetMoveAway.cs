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
                blackboard.moveToPosition = agent.transform.position + -new Vector3(agent.player.transform.position.x - agent.transform.position.x,
                    agent.transform.position.y, agent.player.transform.position.z - agent.transform.position.z ).normalized * distance; 
                break;
            case MoveAwayType.danger:
                    blackboard.moveToPosition = agent.transform.position + -new Vector3(agent.dangerObject.transform.position.x - agent.transform.position.x,
                    agent.transform.position.y, agent.dangerObject.transform.position.z - agent.transform.position.z).normalized * distance;
                break;

        }

        return child.Update();
    }
}
