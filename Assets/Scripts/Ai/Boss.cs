using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Boss Variables")]
    [HideInInspector] public Ability curAbility;
    public float rangedDeterence;
    public float repeatDeterence;
    public float distanceMultiplier;
    public float innerRange;
    public Loadout[] loadouts;
    int loadoutIndex;

    public void GetAbility()
    {
        float highWeight = 0;
        for(int i = 0; i < loadouts[loadoutIndex].abilities.Count(); i++)
        {
            float weight = 100;
            if (caster.caster.abilities[i] == curAbility)
                weight -= repeatDeterence;

            if(caster.caster.abilities[i].GetType() == typeof(RangedAbility))
            {
                float dist = Vector3.Distance(transform.position, player.transform.position) * distanceMultiplier;
                if (dist > innerRange)
                    weight += dist;
                else
                    weight -= rangedDeterence;
            }

            if (caster.caster.abilities[i].GetType() == typeof(MeleeAttackAbility))
            {
                weight -= Vector3.Distance(transform.position, player.transform.position) * distanceMultiplier;
            }

            if (weight > highWeight)
            {
                highWeight = weight;
                curAbility = caster.caster.abilities[i];    
            }
        }
    }

    public void ChangeLoadout(int index)
    {
        loadoutIndex = index;
    }
}
