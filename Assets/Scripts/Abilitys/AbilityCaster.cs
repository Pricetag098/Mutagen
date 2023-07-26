using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public Ability[] abilities;
    public Animator animator;
    public Health ownerHealth;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var ability in abilities)
        {
            ability.Equip(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var ability in abilities)
        {
            ability.Tick();
        }
    }

    

    public virtual void CastAbility(int index,Ability.CastData castData)
    {
        abilities[index].Cast(castData);
    }
}
