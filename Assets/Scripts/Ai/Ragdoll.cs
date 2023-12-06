using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
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
    public Optional<SecondOrderFollower> follower;
    public Optional<SecondOrderFacer> facer;
    public Renderer[] render;
    public GameObject pickupPrefab;
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

    void SetDrop()
     {

        GameObject go = Instantiate(pickupPrefab, transform.position, new Quaternion(0,0,0,0));
        go.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        AbilityPickupInteractable pickup = go.GetComponentInChildren<AbilityPickupInteractable>();

        Ability[] droppedAbilities = new Ability[pickup.abilitys.Length];
        int attemptCount = 0;
        bool full = false;
        int[] usedIndex = new int[pickup.abilitys.Length];
        List<int> usedList = new List<int>();

        while (!full)
        {
            int index = Random.Range(0, agent.manager.dropPool.abilities.Length);

            bool assigned = false;
            //avoid assigning abilities the player already has
            for (int i = 0; i < usedIndex.Length; i++)
            {
                if (droppedAbilities[i] != null)
                {
                    for (int j = 0; j < agent.player.caster.abilities.Length; j++)
                    {
                        if (index == usedIndex[i])
                        {
                            if (agent.player.caster.abilities[j])
                            {
                                assigned = true;
                            }
                        }

                    }
                }
            }
            if (!assigned)
            {
                for (int i = 0; i < droppedAbilities.Length; i++)
                {
                    //find slot that hasnt been assigned
                    if (droppedAbilities[i] == null)
                    {
                        droppedAbilities[i] = agent.manager.dropPool.abilities[index];
                        usedIndex[i] = index;
                        break;
                    }
                }
            }

            //count attempts to assign not used ability, fail after a number of attempts-
            //in order to avoid soft locking the game
            attemptCount++;

            if (attemptCount >= 25)
            {
                //for (int i = 0; i < droppedAbilities.Length; i++)
                //{
                //    if (droppedAbilities[i] == null)
                //    {
                //        droppedAbilities[i] = agent.manager.dropPool.abilities[index];
                //    }
                //}

                for(int i = 0; i < agent.manager.dropPool.abilities.Length; i++)
                {
                    for(int j = 0; j < droppedAbilities.Length; j++)
                    {
                        if (agent.manager.dropPool.abilities[i] != agent.caster.caster.abilities[j] && 
                            agent.manager.dropPool.abilities[i] != droppedAbilities[j])
                        {
                            if(droppedAbilities[j] == null)
                            {
                                droppedAbilities[j] = agent.manager.dropPool.abilities[i];
                            }
                        }
                    }

                }
            }

            int count = 0;
            for (int i = 0; i < droppedAbilities.Length; i++)
            {
                if (droppedAbilities[i] != null)
                {
                    count++;
                    continue;
                    //break;
                }
                //else
                //{
                //    count++;
                //}

            }
            if (count == droppedAbilities.Length)
            {
                full = true;
            }
        }

        //enable interactable

        //pickup.enabled = true;
        pickup.SetAbilities(droppedAbilities);
        //pickup.GetComponent<Collider>().enabled = true;

        return;
    }

    void OnDie(DamageData data)
    {
        dead = true;

        //disable other colliders
        hb.transform.GetComponent<Collider>().enabled = false;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if(follower.Enabled)
            follower.Value.enabled = false;
            if(facer.Enabled)
            facer.Value.enabled = false;

            Collider col;
            if (transform.parent.GetChild(i).TryGetComponent<Collider>(out col))
            {
                col.enabled = false;
            }
        }

        //disable components
        if(agent.anim.Enabled)
        agent.anim.Value.enabled = false;
        agent.agent.enabled = false;
        if(agent.behaviourTree.Enabled)
        agent.behaviourTree.Value.enabled = false;
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
        if (!agent.manager.guaranteeDrop || !agent.manager.DropCheck())
        {
            Vector3 forceDirection = new Vector3(transform.position.x + Random.Range(-ragdollSideForce, ragdollSideForce),
                    Random.Range(0, ragdollUpForce), transform.position.y + Random.Range(-ragdollSideForce, ragdollSideForce));
            rb.AddForce(forceDirection, ForceMode.Impulse);
        }
        else
        {
            SetDrop();
            //droppingAbility = true;
        }
    }
}
