using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDangerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.dangerObject = this;
            enemy.isInDanger = true;

        }
    }
}
