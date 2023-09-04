using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ability;

public class HitBox : MonoBehaviour
{
    public float multi = 1;

    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        health = GetComponentInParent<Health>();
    }
    public void OnHit(DamageData data)
    {
        data.damage *= multi;
        health.TakeDmg(data);
    }

}
