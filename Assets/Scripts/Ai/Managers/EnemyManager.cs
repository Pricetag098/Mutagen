using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    public List<Enemy> enemyList = new List<Enemy>();
    public List<EnemyManager> managers = new List<EnemyManager>();
    public FloatingTextManager floatingTextManager;
    public PlayerAbilityCaster player;

    [Header("DropStats")]
    public bool guaranteeDrop = true;
    [Range(0, 100)] public float dropChance;
    public Ability[] dropPool;
    bool hasDropped;

    [Header("Element")]
    int elementIndex;
    public Optional<Element> assignedElement;
    public Material[] elementColours;

    //detection stat
    bool activated;

    //stalker stats/refs
    [HideInInspector] public List<Enemy> inFront = new List<Enemy>();
    public float seperateDist;
    public int moveCount;
    [HideInInspector] public List<Enemy> moving;

    public enum Element
    {
        Normal,
        Light,
        Gravity,
        Tech
    }

    protected void Awake()
    {
        //randomly assigned which element the deers and birds use
        elementIndex = Random.Range(0, 3);

        //adds each enemy to a list
        for (int i = 0; i < transform.childCount; i++)
        {
            Add(transform.GetChild(i).GetComponentInChildren<Enemy>());
        }
        //deactives each of them to allow idle anims
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].Deactivate();
        }

        //find all managers in scene and add to list
        EnemyManager[] list = FindObjectsOfType<EnemyManager>();
        for(int i = 0; i < list.Length; i++)
        {
            managers.Add(list[i]);
        }
    }

    //gets the average direction of each enemy in the list to allow for decluttering
    public Vector3 groupDir(Enemy agent)
    {
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != agent)
            {
                float dist = Vector3.Distance(agent.transform.position, enemyList[i].transform.position);
                if(dist < seperateDist)
                    dir += agent.transform.position - enemyList[i].transform.position;
            }
        }

        return dir.normalized;
    }

    public bool DropCheck()
    {
        //if already assigned a drop enemy, return
        if (hasDropped)
            return false;

        float dropping = Random.Range(0, 100);

        //if last enemy drop ability else random
        if (enemyList.Count == 1)
        {
            hasDropped = true;
        }
        else if (dropping < dropChance)
        {
            hasDropped = true;
        }

        Debug.Log(hasDropped);
        return hasDropped;
    }

    public void Add(Enemy agent)
    {
        //checks if enemy was not removed from moving list
        foreach(Enemy enemy in moving)
        {
            if (enemy == agent)
                moving.Remove(agent);
        }

        //assigns the agents variables
;       enemyList.Add(agent);
        agent.manager = this;
        agent.GetComponent<FloatingTextTarget>().textManager = floatingTextManager;
        agent.player = player;

        if(agent.randoms.Enabled)
            agent.Randomize();

        EnemyAbilityCaster caster = agent.GetComponent<EnemyAbilityCaster>();
        //allows for testing of specific elements
        if (assignedElement.Enabled)
        {
            int value = (int)assignedElement.Value;
            if(agent.pipeColourChanger.Enabled)
            agent.pipeColourChanger.Value.Change(elementColours[value]);
            if (caster.loadoutVariations.Count() -1 > elementIndex)
                caster.AssignLoadout(caster.loadoutVariations[value]);
            else
                caster.AssignLoadout(caster.loadoutVariations[0]);
            return;
        }

        //assign loadout to [elementIndex] if there aren't that many loadouts go to default (0)
        if (caster.loadoutVariations.Count() > elementIndex)
            caster.AssignLoadout(caster.loadoutVariations[elementIndex]);
        else 
            caster.AssignLoadout(caster.loadoutVariations[0]);

        if(agent.pipeColourChanger.Enabled)
        agent.pipeColourChanger.Value.Change(elementColours[elementIndex]);
    }

    public void Remove(Enemy agent)
    {
        enemyList.Remove(agent);

        int emptyCount = 0;

        for (int i = 0; i < managers.Count; i++)
        {
            if (managers[i].enemyList.Count <= 0)
            {
                emptyCount++;
            }
        }

        if(emptyCount == managers.Count)
        {
            Debug.Log("Empty");
            //open exit
        }
    }

    //checks if agents need to start flanking player to avoid cluttering
    private void FixedUpdate()
    {
        if (inFront.Count > moveCount)
        {
            MoveAgent(inFront.Last());
        }
    }

    void MoveAgent(Enemy agent)
    {
        inFront.Remove(agent);
        moving.Add(agent);
        agent.flanking = true;
        agent.Flank();
    }

    //on player collision allow enemies to start attacking 
    private void OnTriggerEnter(Collider collision)
    {
        HitBox player;
        if(collision.gameObject.TryGetComponent<HitBox>(out player))
        {
            //Debug.Log("Hit");
            if (activated)
                return;

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].enabled = true;
                enemyList[i].Activate();
            }
            activated = true;
        }
    }
}
