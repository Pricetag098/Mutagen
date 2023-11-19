using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    rotating,
    ramming
}

public class MovingPoint : MonoBehaviour
{
    public MovementType movementType;
    public Transform rotating;
    public Vector3 startPos { get; private set; }
    public float speed;
    public float distanceFromCentre = 30f;

    public GameObject[] rammingPoints;

    //testing
    public float testDeg;

    private void Start()
    {
        startPos = transform.position;

        for (int i = 0; i < rammingPoints.Length; i++)
        {
            rammingPoints[i].transform.position = new Vector3(distanceFromCentre * Mathf.Cos(testDeg * i) + startPos.x,
                                                    startPos.y, distanceFromCentre * Mathf.Sin(testDeg * i) + startPos.z);
        }
    }

    private void FixedUpdate()
    {
        switch (movementType)
        {
            case MovementType.rotating:
                rotating.position = transform.position + new Vector3(-Mathf.Cos(Time.time * speed) * distanceFromCentre,
                    0, Mathf.Sin(Time.time * speed) * distanceFromCentre);
                break;

            case MovementType.ramming:


                break;
        }



    }

    private void OnDrawGizmos()
    {

    }
}
