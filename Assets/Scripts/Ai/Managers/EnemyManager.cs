using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector] public List<Enemy> inFront = new List<Enemy>();
    public int moveCount;
    [HideInInspector] public List<Enemy> moving;
    public FloatingTextManager floatingTextManager;
    public PlayerAbilityCaster player;
    public Optional<Loadout> assignedLoadout;
    int elementIndex;

    protected void Awake()
    {
        elementIndex = Random.Range(0, 2);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.active)
                Add(transform.GetChild(i).GetComponentInChildren<Enemy>());
        }
    }

    

    public void Add(Enemy agent)
    {
        enemyList.Add(agent);
        agent.manager = this;
        agent.GetComponent<FloatingTextTarget>().textManager = floatingTextManager;
        agent.player = player;

        if (assignedLoadout.Enabled)
        {
            agent.caster.AssignLoadout(assignedLoadout.Value);
            return;
        }

        EnemyAbilityCaster caster = agent.GetComponent<EnemyAbilityCaster>();

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
    }

    void MoveAgent(Enemy agent)
    {
        inFront.Remove(agent);
        moving.Add(agent);
        agent.flanking = true;
        agent.Flank();
    }
}
