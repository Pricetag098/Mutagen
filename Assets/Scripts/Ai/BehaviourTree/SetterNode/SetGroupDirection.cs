using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGroupDirection : SetterNode
{
    public float groupDistance;
    public float spreadDistance;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    Vector3 groupDirection()
    {
        Vector3 dir = Vector3.zero;

        for(int i = 0; i < manager.enemyList.Count; i++)
        {
            if(manager.enemyList[i] != agent)
            {
                float dist = Vector3.Distance(manager.enemyList[i].transform.position, agent.transform.position);
                //if (dist < groupDistance)
                //{
                    dir += new Vector3(agent.transform.position.x - manager.enemyList[i].transform.position.x, 0,
                        agent.transform.position.z - manager.enemyList[i].transform.position.z).normalized;
                //}
            }
        }
        return (agent.transform.position + (dir.normalized * spreadDistance));
    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition = groupDirection();
        return child.Update();
    }
}
