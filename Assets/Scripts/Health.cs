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
    public Action OnDeath;
    public Action OnHit;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        iFrames = Mathf.Clamp(iFrames - Time.deltaTime, 0, maxIFrames);
    }
    public void TakeDmg(float dmg)
    {
        if (iFrames > 0)
            return;
        health = Mathf.Clamp(health -dmg,0,maxHealth);
        if(OnHit != null)
        OnHit();
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
}
