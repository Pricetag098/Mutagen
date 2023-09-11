using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityCaster : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public AbilityCaster caster;
    [SerializeField] Loadout curLoadout; //shown for debugging
    [Tooltip("Keep loadouts in order of Normal, Light, Gravity, Tech")]
    public Loadout[] loadoutVariations;
    Enemy enemy;
    Transform player;
    public Transform castOrigin;
    [HideInInspector] public Ability curAbility;

    [Header("Stats")]
    public float projectileDeviation;
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

            if (caster.abilities[i].GetType() == typeof(RangedAbility))
            {
                float dist = Vector3.Distance(transform.position, player.transform.position) * distanceMultiplier;
                if (dist > innerRange)
                    weight += dist;
                else
                    weight -= rangedDeterence;
            }

            if (caster.abilities[i].GetType() == typeof(MeleeAttackAbility))
            {
                //caster.abilities[i].
                weight -= Vector3.Distance(transform.position, player.transform.position) * distanceMultiplier;
            }
            float chanceDeduction = Random.Range(0, chanceDeductionRange);
            weight -= chanceDeduction;

            if (weight > highWeight)
            {
                highWeight = weight;
                curAbility = caster.abilities[i];
                Debug.Log(curAbility.name);
            }
        }
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
