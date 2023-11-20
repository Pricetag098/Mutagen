using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderFacer : MonoBehaviour
{
	[Range(0f, 15f)] public float f;
	[Range(0f, 2f)] public float zeta;
	[Range(-2f, 2f)] public float r;
	public bool updateVals = true;
	SecondOrderDynamics x, y, z;

	public bool projectToFloor;
	public LayerMask floor;
	public Vector3 targetVec;

    public bool fixedUpdate;
	public Optional<Transform> target;
	private void Start()
	{
		if (target.Enabled)
		{
			x = new SecondOrderDynamics(f, zeta, r, target.Value.forward.x);
			y = new SecondOrderDynamics(f, zeta, r, target.Value.forward.y);
			z = new SecondOrderDynamics(f, zeta, r, target.Value.forward.z);
		}
		else
		{
            x = new SecondOrderDynamics(f, zeta, r, transform.forward.x);
            y = new SecondOrderDynamics(f, zeta, r, transform.forward.y);
            z = new SecondOrderDynamics(f, zeta, r, transform.forward.z);
        }
		
	}

	private void Update()
	{
		if (!fixedUpdate)
		{
			Process(Time.deltaTime);
		}
	}
	private void FixedUpdate()
	{
		if (fixedUpdate)
		{
			Process(Time.fixedDeltaTime);
		}


	}

	void Process(float timeStep)
	{
		if (updateVals)
		{
			x.UpdateKVals(f, zeta, r);
			y.UpdateKVals(f, zeta, r);
			z.UpdateKVals(f, zeta, r);
		}
		if(target.Enabled)
			targetVec = target.Value.forward;

		if (projectToFloor)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, floor))
			{
				targetVec = Vector3.ProjectOnPlane(targetVec, hit.normal);
				transform.up = hit.normal;
			}
		}

		Vector3 newPos = new Vector3(
			x.Update(timeStep, targetVec.x),
			y.Update(timeStep, targetVec.y),
			z.Update(timeStep, targetVec.z));

		
		transform.forward = newPos.normalized;
	}
}
