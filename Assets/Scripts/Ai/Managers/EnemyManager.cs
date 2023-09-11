using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    public List<Enemy> enemyList = new List<Enemy>();
    public FloatingTextManager floatingTextManager;
    public PlayerAbilityCaster player;

    [Header("Element")]
    int elementIndex;
    public Optional<Element> assignedElement;

    [Header("Stats")]
    public float detectionRadius;
    bool activated;

    //stalker stats/refs
    [HideInInspector] public List<Enemy> inFront = new List<Enemy>();
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
        elementIndex = Random.Range(0, 2);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.active)
                Add(transform.GetChild(i).GetComponentInChildren<Enemy>());
        }

        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].enabled = false;
        }
    }

    

    public void Add(Enemy agent)
    {
        enemyList.Add(agent);
        agent.manager = this;
        agent.GetComponent<FloatingTextTarget>().textManager = floatingTextManager;
        agent.player = player;

        EnemyAbilityCaster caster = agent.GetComponent<EnemyAbilityCaster>();

        if (assignedElement.Enabled)
        {
            int value = (int)assignedElement.Value;
            caster.AssignLoadout(caster.loadoutVariations[value]);
            return;
        }

        //assign loadout to [elementIndex] if there aren't that many loadouts go to default (0)
        if (caster.loadoutVariations.Count() > elementIndex)
            caster.AssignLoadout(caster.loadoutVariations[elementIndex]);
        else 
            caster.AssignLoadout(caster.loadoutVariations[0]);

    }

    private void FixedUpdate()
    {



        if (inFront.Count > moveCount)
        {
            MoveAgent(inFront.Last());
        }

        if (activated)
            return;

        if (Vector3.Distance(transform.position, player.transform.position) < detectionRadius)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].enabled = true;
            }
        }
    }

    void MoveAgent(Enemy agent)
    {
        inFront.Remove(agent);
        moving.Add(agent);
        agent.flanking = true;
        agent.Flank();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
