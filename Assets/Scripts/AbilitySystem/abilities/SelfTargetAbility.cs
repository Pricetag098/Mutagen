using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/SelfTarget")]
public class SelfTargetAbility : Ability
{

    [SerializeField] float damageAmount;
    
    protected override void DoCast(CastData data)
    {
        caster.ownerHealth.TakeDmg(CreateDamageData(damageAmount*Time.deltaTime));
    }
}
