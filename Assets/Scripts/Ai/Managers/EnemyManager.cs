using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemyList;
    public int moveCount;
    [HideInInspector] public List<Enemy> moving;

    private void Start()
    {
        enemyList = FindObjectsOfType<Enemy>();
    }
}
