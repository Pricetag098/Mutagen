using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Health parentHealth;
    HitBox hitBox;
    [Header("Stats")]
    [SerializeField] float maxPartHealth;
    float curPartHealth;


    private void Start()
    {
        curPartHealth = maxPartHealth;
        hitBox = GetComponent<HitBox>();
        hitBox.isBroken += isBroken; 
    }

    public bool isBroken(DamageData data)
    {
        curPartHealth -= data.damage;

        if (curPartHealth <= 0)
        {
            return true;
        }
        return false;
    }

}
