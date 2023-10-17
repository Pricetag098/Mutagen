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
    bool dead;
    bool droppingAbility;
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

        if (!agent.manager.DropCheck() && !droppingAbility)
        {
            droppingAbility = true;
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
            pickup.GetComponent<Collider>().enabled = true;

            return;
        }
        if (droppingAbility)
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

        //enable new colliders and turn on kinematic
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Collider>().enabled = true;
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }

    }
}
