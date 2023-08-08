using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemyList;
    public int moveCount;
    [HideInInspector] public List<Enemy> inFront = new List<Enemy>();
    [HideInInspector] public List<Enemy> moving;

    private void FixedUpdate()
    {
        if(inFront.Count > moveCount)
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
