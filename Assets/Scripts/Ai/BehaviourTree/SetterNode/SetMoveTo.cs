using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    player,
    self,
    away_danger,
    group
};

public class SetMoveTo : SetterNode
{
    public bool away;
    public TargetType type;
    public float awayDistance;
    

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        blackboard.awayPosition = Vector3.zero;
    }    

    protected override State OnUpdate()
    {
        if (!away) //towards target
        {
            switch (type)
            {
                case TargetType.player:
                    blackboard.moveToPosition = player.transform.position;
                    break;
                case TargetType.self:
                    blackboard.moveToPosition = agent.transform.position;
                    break;
            }
            return child.Update();
        }

        //away from target
        switch (type)
        {
            case TargetType.player:
                {
                    if(blackboard.awayPosition != Vector3.zero)
                    {
                        if (Vector3.Distance(agent.transform.position, blackboard.awayPosition) < 2f)
                        {
                            blackboard.awayPosition = GetFleePosition();
                            blackboard.moveToPosition = blackboard.awayPosition;
                        }
                        else return child.Update();
                    }
                    else
                    {
                        blackboard.awayPosition = GetFleePosition();
                        blackboard.moveToPosition = blackboard.awayPosition;
                    }
                    break;
                }
            case TargetType.away_danger:
                blackboard.moveToPosition = agent.transform.position + -(agent.dangerObject.transform.position - agent.transform.position).normalized;
                break;
            case TargetType.group:
                blackboard.moveToPosition = agent.transform.position + manager.groupDir(agent) * awayDistance;
                break;
        }

        return child.Update();
    }

    Vector3 GetFleePosition()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(agent.transform.position + Random.onUnitSphere * awayDistance, out hit, awayDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Angle(agent.transform.position - player.transform.position, agent.transform.position + hit.position) > 90)
        {
            NavMesh.SamplePosition(agent.transform.position + Random.onUnitSphere * awayDistance, out hit, awayDistance, NavMesh.AllAreas);

            i++;
            if (i >= 30)
                break;
        }
        return hit.position;
    
    }
}
