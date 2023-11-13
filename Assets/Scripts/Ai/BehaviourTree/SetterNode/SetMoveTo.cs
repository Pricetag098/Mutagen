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
        //if(Vector3.Distance(agent.transform.position, blackboard.awayPosition) < 5)
        //blackboard.awayPosition = Vector3.zero;
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
                    ////if already running away, continue
                    //if (blackboard.awayPosition != Vector3.zero)
                    //{
                    //    float dist = Vector3.Distance(agent.transform.position, blackboard.awayPosition);
                    //    Debug.Log(agent.transform.position + " " + blackboard.awayPosition);
                    //    if (dist < 10f)
                    //    {
                    //        blackboard.awayPosition = GetFleePosition();
                    //        blackboard.moveToPosition = blackboard.awayPosition;
                    //    }
                    //    else return child.Update();
                    //}
                    //else
                    {
                        blackboard.awayPosition = GetFleePosition();
                        blackboard.moveToPosition = blackboard.awayPosition;
                    }
                    break;
                }
            case TargetType.group:
                blackboard.moveToPosition = agent.transform.position + manager.groupDir(agent) * awayDistance;
                break;
        }

        return child.Update();
    }

    Vector3 GetFleePosition()
    {
        //NavMeshHit hit;
        //NavMesh.SamplePosition(agent.transform.position + Random.onUnitSphere * awayDistance, out hit, awayDistance, NavMesh.AllAreas);

        //Vector3 newPos = agent.transform.position;
        //newPos.x += Random.Range(-awayDistance, awayDistance); newPos.z += Random.Range(-awayDistance, awayDistance);


        //int i = 0;
        //while (Vector3.Angle(agent.transform.position - player.transform.position, agent.transform.position + hit.position) > 90)
        //{
        //    NavMesh.SamplePosition(agent.transform.position + Random.onUnitSphere * awayDistance, out hit, awayDistance, NavMesh.AllAreas);

        //    i++;
        //    if (i >= 30)
        //        break;
        //}
        //Vector3 temp = hit.position;
        //temp.y = agent.transform.position.y;

        Vector3 temp = agent.transform.position + (agent.transform.position - player.transform.position).normalized * awayDistance;

        return temp;    
    }
}
