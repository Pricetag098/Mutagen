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
	bool canUse = true;
	protected Vector3 startPoint,endPoint,direction;
	Transform castOrigin;
	[SerializeField] AimAssist aimAssist;
	[SerializeField] float gcRange;
	[SerializeField] LayerMask layer;
	public string animationTrigger;
	protected override void OnEquip()
	{
		maxStam = rechargeTime * Casts;
		dashVel = dashDistance / dashTime;
		dashTimer = new Timer(dashTime,false);
		stam = maxStam;
	}
	public override void OnTick()
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
		if(stam == maxStam)
		{
			canUse = true;
		}
	}

	public override float GetCoolDownPercent()
	{
		return stam / maxStam;
	}
	protected override void DoCast(CastData data)
	{
		if (dashing || !canUse)
			return;
		
			
		if(stam > rechargeTime)
		{
			castOrigin = data.effectOrigin;
			stam -= rechargeTime;
			if(stam < rechargeTime)
			{
				canUse = false;
			}
			dashTimer.Reset();
			caster.ownerHealth.AddIFrames(iFramesGranted);

			direction = aimAssist.GetAssistedAimDir(data.moveDirection,caster.transform.position,dashVel);
			direction.y = 0;
			if(Physics.Raycast(caster.transform.position,Vector3.down,out RaycastHit hit,gcRange,layer))
				direction = Vector3.ProjectOnPlane(direction,hit.normal);
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
			caster.animator.Value.SetTrigger(animationTrigger);
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
