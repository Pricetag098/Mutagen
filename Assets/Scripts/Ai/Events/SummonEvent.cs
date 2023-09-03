using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/Summon"))]
public class SummonEvent : Event
{
    public GameObject summonPrefab;
    public int summonCount;
    public Transform[] summonPositions; //might change later
    GameObject enemy;

    public override void Play(Enemy agent)
    {
        base.Play(agent);
        for (int i = 0; i < summonCount; i++)
        {
            enemy = Instantiate(summonPrefab);
            enemy.transform.position = summonPositions[i].position;
            enemy.GetComponent<Enemy>().player = agent.player;
        }
    }

}
