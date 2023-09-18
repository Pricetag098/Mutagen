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
    Enemy agent;
    public Renderer[] render;

    [Header("Stats")]
    public float fadeRate;
    public float ragdollForce;
    bool dead;
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
            Debug.Log("Fading");
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

        Debug.Log("Ragdoll");

        //disable other colliders
        hb.transform.GetComponent<Collider>().enabled = false;

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

            Collider col;
            if (transform.parent.GetChild(i).TryGetComponent<Collider>(out col))
            {
                col.enabled = false;
            }
        }

        //enable new colliders and turn on kinematic
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Collider>().enabled = true;
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            Vector3 random = new Vector3(Random.Range(0, ragdollForce), 0, Random.Range(0, ragdollForce));
            rb.AddForce((random + Vector3.up) * ragdollForce, ForceMode.Impulse);
        }

        //disable components
        agent.anim.enabled = false;
        agent.agent.enabled = false;
        agent.behaviourTree.enabled = false;
        agent.enabled = false;


    }
}
