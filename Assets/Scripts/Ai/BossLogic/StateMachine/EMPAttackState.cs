using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAttackState : AttackState
{
    [Header("Ability Stats")]
    [SerializeField] int abilityIndex = 2;

    [Header("Multicast Stats")]
    [SerializeField] int castAmount = 3;
    [SerializeField] float waitTimer;
    float waitTime;
    bool waiting;
    float castCount;


    public override void OnEnter()
    {
        base.OnEnter();
        manager.movementPoint.transform.position = manager.player.transform.position;

        castCount = 0;
    }

    public override void OnExit()
    {
        base.OnExit();

        manager.movementPoint.transform.position = manager.movementPoint.startPos;

        castCount = 0;
    }

    public override void Tick()
    {
        //movement



        //ability casting
        if (Time.time - delayTimer < delayAmount || castCount >= castAmount)
            return;

        //waiting
        if (waiting)
        {
            if (Time.time - waitTimer > waitTime)
            {
                waiting = false;
            }
            else return;
        }

        Ability.CastData data = manager.caster.CreateCastData();

        //if (manager.agent.pipeColourChanger.Enabled)
        //    manager.agent.AttackEffect();

        //change cast origins to random points within cast area
        for (int i = 0; i < manager.castOrigins.Length; i++)
            manager.caster.caster.CastAbility(abilityIndex, data);

        castCount++;
    }


}
