using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderFollower : MonoBehaviour
{

	[Range(0f, 15f)] public float f;
	[Range(0f, 2f)] public float zeta;
	[Range(-2f, 2f)] public float r;
	public bool updateVals = true;
	SecondOrderDynamics x,y,z;


	public bool fixedUpdate;
	public Transform target;
	private void Start()
	{
		x = new SecondOrderDynamics(f, zeta, r, target.position.x);
		y = new SecondOrderDynamics(f, zeta, r, target.position.y);
		z = new SecondOrderDynamics(f, zeta, r, target.position.z);
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
				x.Update(Time.deltaTime, target.position.x),
				y.Update(Time.deltaTime, target.position.y),
				z.Update(Time.deltaTime, target.position.z));
			transform.position = newPos;
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
				x.Update(Time.fixedDeltaTime, target.position.x),
				y.Update(Time.fixedDeltaTime, target.position.y),
				z.Update(Time.fixedDeltaTime, target.position.z));
			transform.position = newPos;
		}

		
	}
	
}
