using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityCaster : MonoBehaviour
{
    [Header("References")]
    AbilityCaster caster;
    public PlayerAbilityCaster player;
    public BehaviourTreeRunner behaviour;
    public Transform castOrigin;


    private void Start()
    {
        caster = GetComponent<AbilityCaster>();
    }


    Ability.CastData CreateCastData()
    {
        Ability.CastData data = new Ability.CastData();
        data.origin = transform.position;
        //deviation math
        data.aimDirection = player.transform.position;// new Vector3();
        data.moveDirection = transform.position + transform.forward;
        data.effectOrigin = castOrigin;
        return data;
    }
}
