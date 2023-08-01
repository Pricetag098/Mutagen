using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityCaster : MonoBehaviour
{
    [Header("References")]
    AbilityCaster caster;
    public GameObject player;
    //public BehaviourTreeRunner behaviour;
    public Transform castOrigin;
    public Vector3 aimDir;


    private void Start()
    {
        caster = GetComponent<AbilityCaster>();
    }


    public Ability.CastData CreateCastData()
    {
        Ability.CastData data = new Ability.CastData();
        data.origin = transform.position;
        //deviation math
        aimDir = transform.position - transform.forward;
        data.aimDirection = aimDir;
        data.moveDirection = transform.position + transform.forward;
        data.effectOrigin = castOrigin;
        return data;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        aimDir = transform.position - transform.forward;
        Gizmos.DrawLine(transform.position, aimDir);
    }
}
