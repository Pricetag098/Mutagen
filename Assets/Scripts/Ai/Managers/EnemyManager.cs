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
    [HideInInspector] public FloatingTextManager textManager;

    protected void Start()
    {
        //List<Enemy> eList = new List<Enemy>();
        //eList.AddRange(FindObjectsOfType<Enemy>());
        //for (int i = 0; i < eList.Count; i++)//foreach(Enemy enemy in eList)
        //{
        //    if (eList[i].behaviourTree.tree == profile)
        //    {
        //        enemyList.Add(eList[i]);
        //    }
        //}
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.active)
            enemyList.Add(transform.GetChild(i).GetComponentInChildren<Enemy>());
        }

        //enemyList.AddRange(transform.GetComponentInChildren<Enemy>());
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
