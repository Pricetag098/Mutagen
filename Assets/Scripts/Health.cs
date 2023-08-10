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

    public delegate void DamageAction(float dmg);
    public Action OnDeath;
    public DamageAction OnHit;

    public List<StatusEffect> effects = new List<StatusEffect>();
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        iFrames = Mathf.Clamp(iFrames - Time.deltaTime, 0, maxIFrames);
        for(int i = effects.Count-1; i >= 0 ; i--)
		{
            effects[i].Tick();
		}
    }
    public void TakeDmg(float dmg)
    {
        if (iFrames > 0)
            return;
        health = Mathf.Clamp(health -dmg,0,maxHealth);
        if(OnHit != null)
        OnHit(dmg);
        if(health <= 0)
		{
            Die();
            
		}
    }

    public void AddIFrames(float amount)
	{
        iFrames = Mathf.Clamp(iFrames + amount, 0, maxIFrames);
    
    }

    void Die()
	{
        //do die stuff
        
        if(OnDeath != null)
        OnDeath();
	}

    public void AddStatusEffect(StatusEffect effect)
	{
		if (!HasStatusEffect(effect))
		{
            StatusEffect ef = Instantiate(effect);
            effects.Add(ef);
            ef.Add(this);
		}
	}

    public bool HasStatusEffect(StatusEffect effect)
	{
        foreach(StatusEffect statusEffect in effects)
		{
            if(statusEffect.GetType() == effect.GetType())
                return true;
		}
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

	private void OnDestroy()
	{
		foreach(StatusEffect effect in effects)
		{
            effect.Remove();
		}
	}
}
