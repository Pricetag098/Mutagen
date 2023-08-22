using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ability;

public class HitBox : MonoBehaviour
{
    public float multi = 1;
    public FloatingTextManager textManager;
    public Color critColor;
    public float textDuration;

    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        health = GetComponentInParent<Health>();

        textManager = FindObjectOfType<FloatingTextManager>(); //will replace later
    }
    public void OnHit(DamageData data)
    {
        data.target = this.gameObject;
        data.damage *= multi;
        
        health.TakeDmg(data);
        textManager.Show(data);
    }

}
