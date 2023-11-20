using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/States/AttackStates"))]
public class AttackState : State
{
    [Header("Ability Stats")]
    [SerializeField] int abilityIndex;
    [SerializeField] bool deviatedDirection;
    [SerializeField] float castTime;
    [Tooltip("How long after the attack before the boss can attack again")]
    [SerializeField] float setActionCooldown;
    float timer;
    Ability ability;

    [Header("Behaviour Stats")]
    public float cooldown;
    [SerializeField] float agentSpeed;
    float baseSpeed;
    [SerializeField] protected bool delay;
    [SerializeField] protected float delayAmount;
    protected float delayTimer;

    public override void OnEnter()
    {
        //movement set
        //manager.movementPoint.speed = agentSpeed;
        //baseSpeed = manager.nav.speed;
        //manager.nav.speed = agentSpeed;
        //manager.nav.SetDestination(manager.nav.transform.position);
        manager.movementTarget = manager.movementPoint.rotating.transform;
        manager.casting = true;
        ability = manager.caster.caster.abilities[abilityIndex];

        //set timers
        timer = Time.time;

        if(delay)
            delayTimer = Time.time;
    }

    public override void OnExit()
    {
        manager.specialAttackCooldown = cooldown;
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

        if (ability.castType == Ability.CastTypes.hold)
        {
            for (int i = 0; i < manager.castOrigins.Length; i++)
                manager.caster.caster.CastAbility(abilityIndex, data);
            if (Time.time - timer > castTime)
            {
                //how long after this ability finishes until the boss can do another attack
                manager.actionCooldown = setActionCooldown;
                manager.actionTimer = Time.time;
                //finished casting
                manager.casting = false;
            }
            return;
        }

        for (int i = 0; i < manager.castOrigins.Length; i++)
            manager.caster.caster.CastAbility(abilityIndex, data);

        //how long after this ability finishes until the boss can do another attack
        manager.actionCooldown = setActionCooldown;
        manager.actionTimer = Time.time;

        manager.casting = false;
    }
}
