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


    private void Start()
    {
        caster = GetComponent<AbilityCaster>();
    }


    public Ability.CastData CreateCastData()
    {
        Ability.CastData data = new Ability.CastData();
        data.origin = castOrigin.transform.position;
        //deviation math
        aimDir = new Vector3(player.transform.position.x - transform.position.x, 0,
            player.transform.position.z - transform.position.z).normalized;

        data.aimDirection = aimDir;
        data.moveDirection = transform.position + transform.forward;
        data.effectOrigin = castOrigin;
        return data;
    }

}
