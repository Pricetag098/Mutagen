using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/States/AttackStates"))]
public class AttackState : State
{
    [SerializeField] int abilityIndex;
    [SerializeField] bool deviatedDirection;
    [SerializeField] int castTime;
    float timer;
    Ability ability;

    public override void OnEnter()
    {
        manager.movementPoint.speed = 0;
        manager.nav.SetDestination(manager.nav.transform.position);
        manager.casting = true;
        ability = manager.caster.caster.abilities[abilityIndex];
        timer = Time.time;
    }

    public override void OnExit()
    {

    }

    public override void Tick()
    {
        Ability.CastData data = manager.caster.CreateCastData();
        if (deviatedDirection)
        {
            float dev = Random.Range(-manager.caster.projectileDeviation, manager.caster.projectileDeviation);

            data.aimDirection = new Vector3((manager.player.transform.position.x - manager.transform.position.x) + dev,
                0, (manager.transform.position.z - manager.transform.position.z) + dev).normalized;
        }

        //if uses dash's put here
        if(manager.agent.pipeColourChanger.Enabled)
        manager.agent.AttackEffect();

        Debug.Log("Attack");

        if(ability.castType == Ability.CastTypes.hold)
        {
            manager.caster.caster.CastAbility(abilityIndex, data);
            if (Time.time - timer > castTime)
            {
                //finished casting
                manager.casting = false;
            }
        }
        else
        {
            manager.caster.caster.CastAbility(abilityIndex, data);
            manager.casting = false;
        }

    }
}
