using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPoint : MonoBehaviour
{
    [SerializeField] Transform parent;
    public float speed;
    public float distanceFromCentre = 5f;


    private void Start()
    {
        parent = transform.parent;
    }

    private void FixedUpdate()
    {
        transform.position = parent.position + new Vector3(-Mathf.Cos(Time.time * speed) * distanceFromCentre,
            0, Mathf.Sin(Time.time * speed) * distanceFromCentre);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(parent.position, transform.position);
    }
}
