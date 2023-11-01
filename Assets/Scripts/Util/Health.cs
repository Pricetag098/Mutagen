using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;

    public float iFrames;
    [SerializeField] float maxIFrames = float.PositiveInfinity;
    public delegate void Action();

    public delegate void DamageAction(DamageData data);
    public DamageAction OnDeath;
    public DamageAction OnHit;

    public bool canHaveStatusEffects = true;
    public List<StatusEffect> effects = new List<StatusEffect>();
    public bool dead = false;
    void Update()
    {
        iFrames = Mathf.Clamp(iFrames - Time.deltaTime, 0, maxIFrames);
        for(int i = effects.Count-1; i >= 0 ; i--)
		{
            effects[i].Tick();
		}
    }
    public bool TakeDmg(DamageData data)
    {
        if (iFrames > 0)
            return false;
        health = Mathf.Clamp(health -data.damage,0,maxHealth);
        if(OnHit != null)
        OnHit(data);

        if (health <= 0)
		{
            Die(data);
        }
        return true;
    }

    public void AddIFrames(float amount)
	{
        iFrames = Mathf.Clamp(iFrames + amount, 0, maxIFrames);
    
    }

    public void Respawn()
    {
        dead = false;
        health = maxHealth;
    }
    public void Die(DamageData data)
	{
        if(dead)
            return;
        dead = true;
        //do die stuff
        
        if(OnDeath != null)
        OnDeath(data);
	}

    public void AddStatusEffect(StatusEffect effect)
	{
        if(!canHaveStatusEffects)
            return;
        StatusEffect existingEffect;
		if (HasStatusEffect(effect,out existingEffect) && existingEffect.stacks + effect.stacks <= existingEffect.maxStacks)
		{
            existingEffect.Combine(effect);
            existingEffect.stacks += effect.stacks;
		}
		else
		{
            StatusEffect ef = Instantiate(effect);
            effects.Add(ef);
            ef.Add(this);
        }
	}

    public bool HasStatusEffect(StatusEffect effect,out StatusEffect instance)
	{
        foreach(StatusEffect statusEffect in effects)
		{
            if(statusEffect.GetType() == effect.GetType())
			{
                instance = statusEffect;
                return true;
            }
                
		}
        instance = null;
        return false;
	}

    public void RemoveStatusEffect(StatusEffect statusEffect)
	{
        for(int i = 0; i < effects.Count; i++)
		{
            if(effects[i].GetType() == statusEffect.GetType())
			{
                effects[i].Remove();
                effects.RemoveAt(i);
			}
		}
	}

    public void ClearStatusEffects()
	{
        foreach (StatusEffect effect in effects)
        {
            effect.Remove();
        }
        effects.Clear();
    }

	private void OnDestroy()
	{
		ClearStatusEffects();
	}
}

