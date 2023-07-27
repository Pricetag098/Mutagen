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


	public bool fixedUpdate;
	public Transform target;
	private void Start()
	{
		x = new SecondOrderDynamics(f, zeta, r, target.forward.x);
		y = new SecondOrderDynamics(f, zeta, r, target.forward.y);
		z = new SecondOrderDynamics(f, zeta, r, target.forward.z);
	}

	private void Update()
	{
		if (!fixedUpdate)
		{
			if (updateVals)
			{
				x.UpdateKVals(f, zeta, r);
				y.UpdateKVals(f, zeta, r);
				z.UpdateKVals(f, zeta, r);
			}

			Vector3 newPos = new Vector3(
				x.Update(Time.deltaTime, target.forward.x),
				y.Update(Time.deltaTime, target.forward.y),
				z.Update(Time.deltaTime, target.forward.z));
			transform.forward = newPos;
		}
	}
	private void FixedUpdate()
	{
		if (fixedUpdate)
		{
			if (updateVals)
			{
				x.UpdateKVals(f, zeta, r);
				y.UpdateKVals(f, zeta, r);
				z.UpdateKVals(f, zeta, r);
			}

			Vector3 newPos = new Vector3(
				x.Update(Time.fixedDeltaTime, target.forward.x),
				y.Update(Time.fixedDeltaTime, target.forward.y),
				z.Update(Time.fixedDeltaTime, target.forward.z));
			transform.forward = newPos;
		}


	}
}
