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
	[SerializeField] Optional<VfxSpawnRequest> dashFx;
	float dashVel;
	float maxStam;
	float stam;

	bool dashing;

	Rigidbody rb;
	Timer dashTimer;

	protected Vector3 startPoint,endPoint,direction;
	Transform castOrigin;

	public string animationTrigger;
	protected override void OnEquip()
	{
		maxStam = rechargeTime * Casts;
		dashVel = dashDistance / dashTime;
		dashTimer = new Timer(dashTime,false);
		stam = maxStam;
	}
	public override void Tick()
	{
		
		if (dashing)
		{
			WhileDashing();
			dashTimer.Tick();
			if(dashTimer.complete)
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
			castOrigin = data.effectOrigin;
			stam -= rechargeTime;
			dashTimer.Reset();
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
		if (dashFx.Enabled)
			dashFx.Value.Play(castOrigin.position, direction,castOrigin);

		if (caster.animator.Enabled)
			caster.animator.Value.SetTrigger("Dash");
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
			caster.transform.position = Vector3.Lerp(startPoint, endPoint, dashTimer.Progress);
		}
	}
}
