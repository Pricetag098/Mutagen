using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public enum TargetType
{
    player,
    self,
    away_danger,
    //dash_player,
    //dash_danger
};

public class SetMoveTo : SetterNode
{
    public bool away;
    public TargetType type;
    public float awaySpeed;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

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
                blackboard.moveToPosition = agent.transform.position + -(player.transform.position - agent.transform.position).normalized * awaySpeed;
                break;
            case TargetType.away_danger:
                blackboard.moveToPosition = agent.transform.position + -(agent.dangerObject.transform.position - agent.transform.position).normalized;
                break;
        }

        return child.Update();
    }
}
