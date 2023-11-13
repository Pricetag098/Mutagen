using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ability;

public class HitBox : MonoBehaviour
{
    public float multi = 1;

    public Health health;

    //used for boss
    public float partBrokenMulti = 1.5f;
    public delegate bool PartAction(DamageData damageData);
    public PartAction isBroken;

    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        health = GetComponentInParent<Health>();
    }
    public void OnHit(DamageData data)
    {
        if(isBroken != null)
        {
            if (isBroken(data))
            {
                data.damage *= partBrokenMulti;
                health.TakeDmg(data);
            }
            else
            {
                data.damage *= multi;
                health.TakeDmg(data);
            }
        }
        else
        {
            data.damage *= multi;
            health.TakeDmg(data);
        }
    }
}
