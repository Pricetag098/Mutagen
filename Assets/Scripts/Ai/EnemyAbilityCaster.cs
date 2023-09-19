using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityCaster : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public AbilityCaster caster;
    public Loadout curLoadout;
    [Tooltip("Keep loadouts in order of Normal, Light, Gravity, Tech")]
    public Loadout[] loadoutVariations;
    Enemy enemy;
    Transform player;
    public Transform castOrigin;
    [HideInInspector] public Ability curAbility;

    [Header("Stats")]
    public float projectileDeviation;

    [Header("Decision Stats")]
    public float rangedDeterence;
    public float repeatDeterence;
    public float distanceMultiplier;
    public float innerRange;
    [Range(0f, 100f)]
    public float chanceDeductionRange;

    private void Start()
    {
        caster = GetComponent<AbilityCaster>();
        enemy = GetComponent<Enemy>();
        player = enemy.player.transform;
    }

    //assigned when added to manager list
    public void AssignLoadout(Loadout newLoadout)
    {
        caster = GetComponent<AbilityCaster>();

        curLoadout = Instantiate(newLoadout);
        for (int i = 0; i < curLoadout.abilities.Length; i++)
        {
            caster.abilities[i] = Instantiate(curLoadout.abilities[i]);
            caster.abilities[i].Equip(caster);
        }
    }

    public void GetAbility()
    {
        float highWeight = 0;
        for (int i = 0; i < curLoadout.abilities.Length; i++)
        {
            float weight = 100;
            if (caster.abilities[i] == curAbility)
                weight -= repeatDeterence;

            weight += TypeWeight(caster.abilities[i]);

            float chanceDeduction = Random.Range(0, chanceDeductionRange);
            weight -= chanceDeduction;

            if (weight > highWeight)
            {
                highWeight = weight;
                curAbility = caster.abilities[i];
            }
        }
    }

    float TypeWeight(Ability ability)
    {
        float value = 0;

        RangedAbility ranged = ability as RangedAbility;
        if (ranged)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position) * distanceMultiplier;
            if (dist > innerRange)
                value += dist;
            else
                value -= rangedDeterence;
        }

        MeleeAttackAbility melee = ability as MeleeAttackAbility;
        if (melee)
        {
            if (melee.GetCoolDownPercent() < 0.9f)
                value -= 30;

            value -= Vector3.Distance(transform.position, player.transform.position) * distanceMultiplier;
        }

        DashAbility dash = ability as DashAbility;
        if (dash)
        {
            if (dash.GetCoolDownPercent() < 0.9f)
                value -= 30;
        }

        OrbitAbility orbit = ability as OrbitAbility;
        if (orbit)
        {
            if (orbit.GetCoolDownPercent() < 0.9f)
                value -= 30;
        }

        return value;
    }

    public Ability.CastData CreateCastData()
    {
        Ability.CastData data = new Ability.CastData();
        data.origin = castOrigin.transform.position;
        //aiming and deviation
        float dev = Random.Range(-projectileDeviation, projectileDeviation);
        data.aimDirection = new Vector3((player.position.x - transform.position.x) + dev,0,
            (player.position.z - transform.position.z) + dev).normalized;

        data.moveDirection = Vector3.zero;
        data.effectOrigin = castOrigin;
        return data;
    }
}
