using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAttackState : AttackState
{



    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Tick()
    {
        Ability.CastData data = manager.caster.CreateCastData();
    }


}
