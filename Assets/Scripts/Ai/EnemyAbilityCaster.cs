using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityCaster : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public AbilityCaster caster;
    public GameObject player;
    public Transform castOrigin;
    public Vector3 aimDir;
    public float deviation;


    private void Start()
    {
        caster = GetComponent<AbilityCaster>();
    }


    public Ability.CastData CreateCastData()
    {
        Ability.CastData data = new Ability.CastData();
        data.origin = castOrigin.transform.position;
        float dev = Random.Range(-deviation, deviation);
        aimDir = new Vector3((player.transform.position.x - transform.position.x) + dev,0,
            (player.transform.position.z - transform.position.z) + dev).normalized;

        data.aimDirection = aimDir;
        data.moveDirection = transform.position + transform.forward;
        data.effectOrigin = castOrigin;
        return data;
    }
}
