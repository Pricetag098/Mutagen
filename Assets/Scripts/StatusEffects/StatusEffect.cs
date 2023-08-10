using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
	protected Health health;
	public Sprite icon;
	public string effectName;
	
    public void Add(Health hp)
	{
		health = hp;
		OnAdd();
	}

	public void Remove()
	{
		OnRemove();
	}

	public virtual void Tick()
	{

	}

	protected virtual void OnAdd()
	{

	}

	protected virtual void OnRemove()
	{

	}
}
