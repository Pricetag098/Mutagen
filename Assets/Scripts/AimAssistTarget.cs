using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AimAssistTarget : MonoBehaviour
{
    public float size = 1;
    public float priority = 1;

    public enum VelSources
	{
        still,
        rigidBody,
        ai,
        position
	}
    
    

    [SerializeField] VelSources velSource;

    [SerializeField] Rigidbody rb;
    [SerializeField] NavMeshAgent agent;
    Vector3 lastPos;
    Vector3 velocity;
	private void Awake()
	{
		lastPos = transform.position;
	}
	private void FixedUpdate()
	{
		if(velSource == VelSources.position)
		{
            velocity = (transform.position - lastPos)* Time.fixedDeltaTime;
		}
	}

    public Vector3 GetVelocity()
	{
		switch (velSource) 
        { 
            case VelSources.rigidBody: return rb.velocity;
            case VelSources.ai: return agent.velocity;
            case VelSources.position: return velocity;
            default: return Vector3.zero;
        }
	}
}
