using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityCaster : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public AbilityCaster caster;
    public Loadout loadout;
    Enemy enemy;
    Transform player;
    public Transform castOrigin;
    [Header("Stats")]
    public float projectileDeviation;

    private void Start()
    {
        caster = GetComponent<AbilityCaster>();
        enemy = GetComponent<Enemy>();
        player = enemy.player.transform;
        AssignLoadout(loadout);
    }

    public void AssignLoadout(Loadout newLoadout)
    {
        loadout = Instantiate(newLoadout);
        for (int i = 0; i < loadout.abilities.Length; i++)
        {
            caster.abilities[i] = Instantiate(loadout.abilities[i]);
            caster.abilities[i].Equip(caster);
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
