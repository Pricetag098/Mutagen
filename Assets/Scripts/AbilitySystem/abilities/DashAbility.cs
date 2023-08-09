using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
	[SerializeField,Min(1)] int Casts;
	[SerializeField] float rechargeTime;
	[SerializeField] float iFramesGranted;
	[SerializeField] float dashTime;
	[SerializeField] float dashDistance;
	[SerializeField] float endSpeed;
	float dashVel;
	float maxStam;
	float stam;

	bool dashing;

	Rigidbody rb;
	float dashTimer;

	protected Vector3 startPoint,endPoint,direction;
	
	protected override void OnEquip()
	{
		maxStam = rechargeTime * Casts;
		dashVel = dashDistance / dashTime;
	}
	public override void Tick()
	{
		
		if (dashing)
		{
			WhileDashing();
			dashTimer += Time.deltaTime;
			if(dashTimer > dashTime)
			{
				dashing = false;
				EndDash();
			}

			return;
		}
		stam = Mathf.Clamp(stam + Time.deltaTime, 0, maxStam);
	}

	public override float GetCoolDownPercent()
	{
		return stam / maxStam;
	}
	protected override void DoCast(CastData data)
	{
		if (dashing)
			return;
		
			
		if(stam > rechargeTime)
		{
			stam -= rechargeTime;
			dashTimer = 0;
			caster.ownerHealth.AddIFrames(iFramesGranted);
			direction = data.moveDirection;
			dashing = true;
			DoDash();

            if (OnCast != null)
                OnCast(data);
        }


	}

	protected virtual void DoDash()
	{
		
		startPoint = caster.transform.position;
		endPoint = caster.transform.position + direction * dashDistance;
		
	}

	protected virtual void OnDash()
	{

	}
	protected virtual void EndDash()
	{
		if (caster.rigidbody.Enabled)
		{
			caster.rigidbody.Value.velocity = direction * endSpeed;
		}
	}
	protected virtual void WhileDashing()
	{
		if (caster.rigidbody.Enabled)
		{
			caster.rigidbody.Value.velocity = direction * dashVel;
		}
		else
		{
			caster.transform.position = Vector3.Lerp(startPoint, endPoint, dashTimer/dashTime);
		}
	}
}
