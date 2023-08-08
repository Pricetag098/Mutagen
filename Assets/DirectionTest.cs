using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DirectionTest : MonoBehaviour
{
    public Enemy[] targets;


    private void Update()
    {
        Vector3 dir = Vector3.zero;
        float angle = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            dir += new Vector3(targets[i].transform.position.x - transform.position.x, 0,
                targets[i].transform.position.z - transform.position.z).normalized;
            angle += Vector3.Dot(transform.forward, (targets[i].transform.position - transform.position).normalized);

        }
        dir /= targets.Length;
        angle /= targets.Length;
        Debug.Log("dir = " + dir);
        Debug.Log("Angle = " + angle);
        angle = 0;
        dir = Vector3.zero;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 4);
    }

}
