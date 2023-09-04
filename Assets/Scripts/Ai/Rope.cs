using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    LineRenderer lr;
    public Transform a, b;
    public float length, spring = 10,damper = .2f;
    public int segments;
    SpringJoint ab, bc;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.Lerp(a.position,b.position,.5f);
        lr = GetComponent<LineRenderer>();
        ab = gameObject.AddComponent<SpringJoint>();

        ab.autoConfigureConnectedAnchor = false;
        bc = gameObject.AddComponent<SpringJoint>();
        bc.autoConfigureConnectedAnchor = false;
        ab.connectedAnchor = a.position;
        bc.connectedAnchor = b.position;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().sleepThreshold = -1;
    }

    // Update is called once per frame
    void Update()
    {
        ab.anchor = Vector3.zero;
        bc.anchor = Vector3.zero;
        ab.maxDistance = length/2f;
        bc.maxDistance = length/2f;
        ab.damper = damper;
        bc.damper = damper;
        ab.spring = spring;
        bc.spring = spring;
        ab.connectedAnchor = a.position;
        bc.connectedAnchor = b.position;
        lr.positionCount = segments+2;
        lr.SetPosition(0,a.position);
        lr.SetPosition(segments + 1, b.position);
        for(int i = 1; i < segments + 1; i++)
		{
            lr.SetPosition(i,QaudraticLerp(a.position,transform.position,b.position, (float)i /(float)segments));
		}
    }

    Vector3 QaudraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

}
