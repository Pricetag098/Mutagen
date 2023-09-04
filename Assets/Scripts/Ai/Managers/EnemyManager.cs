using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public BehaviourTree profile;
    public List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector] public List<Enemy> inFront = new List<Enemy>();
    public int moveCount;
    [HideInInspector] public List<Enemy> moving;
    public FloatingTextManager floatingTextManager;
    public PlayerAbilityCaster player;

    protected void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.active)
            enemyList.Add(transform.GetChild(i).GetComponentInChildren<Enemy>());
        }
        //floatingTextManager = FindObjectOfType<FloatingTextManager>();
        for(int i = 0;i < enemyList.Count; i++)
        {
            enemyList[i].manager = this;
            enemyList[i].GetComponent<FloatingTextTarget>().textManager = floatingTextManager;
            enemyList[i].player = player;
        }
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
