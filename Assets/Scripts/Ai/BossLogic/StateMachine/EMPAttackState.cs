using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAttackState : AttackState
{



    public override void OnEnter()
    {
        base.OnEnter();

        manager.movementPoint.transform.position = manager.player.transform.position;
    }

    public override void OnExit()
    {
        base.OnExit();

        manager.movementPoint.transform.position = manager.movementPoint.startPos;
    }

    public override void Tick()
    {
        //movement



        //ability casting
        Ability.CastData data = manager.caster.CreateCastData();
    }


}
