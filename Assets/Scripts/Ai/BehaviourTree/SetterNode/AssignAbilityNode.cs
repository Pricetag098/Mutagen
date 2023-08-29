using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AssignAbilityNode : SetterNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Boss boss = agent as Boss;
        boss.GetAbility();
        return child.Update();
    }
}
