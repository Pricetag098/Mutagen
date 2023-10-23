using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AssignAbilityNode : SetterNode
{
    public Optional<Ability[]> excludedAbility;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        agent.caster.GetAbility(excludedAbility.Value);
        return child.Update();
    }
}
