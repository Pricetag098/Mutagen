using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Ragdoll : MonoBehaviour
{
    [Header("References")]
    Health health;
    public HitBox hb;
    public GameObject[] colliders;
    public SecondOrderFollower follower;
    public SecondOrderFacer facer;
    public Renderer[] render;
    public AbilityPickupInteractable pickup;
    Enemy agent;

    [Header("Stats")]
    public float fadeRate;
    public float ragdollSideForce;
    public float ragdollUpForce;
    bool dead;
    bool droppingAbility;
    bool abilityDropped;
    float alpha;
    

    private void Start()
    {
        health = GetComponent<Health>();
        agent = GetComponent<Enemy>();

        health.OnDeath += OnDie;
    }

    private void Update()
    {
        if (!dead)
            return;

        if (droppingAbility && !abilityDropped)
        {
            abilityDropped = true;
            for(int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<Collider>().enabled = false;
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }


            foreach(Renderer r in render)
            {
                r.material.SetFloat("_RimLight", 1);
            }
            //enable interactable
            pickup.enabled = true;
            pickup.SetAbilities(agent.manager.dropPool);
            pickup.GetComponent<Collider>().enabled = true;

            return;
        }
        if (abilityDropped)
            return;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            SecondOrderFollower follow;
            if (transform.parent.GetChild(i).TryGetComponent<SecondOrderFollower>(out follow))
            {
                follow.enabled = false;
            }

            SecondOrderFacer facer;
            if (transform.parent.GetChild(i).TryGetComponent<SecondOrderFacer>(out facer))
            {
                facer.enabled = false;
            }
        }

        foreach (Renderer r in render)
        {
            alpha -= Time.deltaTime * fadeRate;

            r.material.SetFloat("_Tweak_transparency", alpha);

            if (alpha <= -1)
            {
                Destroy(transform.parent.gameObject);
            }
        }


    }

    void OnDie(DamageData data)
    {
        dead = true;

        //disable other colliders
        hb.transform.GetComponent<Collider>().enabled = false;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            follower.enabled = false;
            facer.enabled = false;

            Collider col;
            if (transform.parent.GetChild(i).TryGetComponent<Collider>(out col))
            {
                col.enabled = false;
            }
        }

        //disable components
        agent.anim.enabled = false;
        agent.agent.enabled = false;
        agent.behaviourTree.enabled = false;
        agent.enabled = false;

        Rigidbody rb = null;
        
        //enable new colliders and turn on kinematic
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Collider>().enabled = true;
            rb = colliders[i].GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

        }

        //if designated as drop source, dont add force to ragdoll
        if (!agent.manager.DropCheck())
        {
            Debug.Log("Hit");
            Vector3 forceDirection = new Vector3(transform.position.x + Random.Range(-ragdollSideForce, ragdollSideForce),
                    Random.Range(0, ragdollUpForce), transform.position.y + Random.Range(-ragdollSideForce, ragdollSideForce));
            rb.AddForce(forceDirection, ForceMode.Impulse);
        }
        else
        {
            droppingAbility = true;
        }
    }
}
