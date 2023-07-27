using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    
    public Ability[] abilities;
    public Optional<Animator> animator;
    public Health ownerHealth;
    new public Optional<Rigidbody> rigidbody;
    

    const string baseAbilityPath = "Abilities/Empty";
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < abilities.Length; i++)
		{
            if(abilities[i] == null)
			{
                abilities[i] = Instantiate(Resources.Load<Ability>(baseAbilityPath));
            }
			else
			{
                abilities[i] = Instantiate(abilities[i]);
            }
            abilities[i].Equip(this);
            
		}
    }

    

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Tick();
        }
        
    }


    public virtual void CastAbility(int index,Ability.CastData castData)
    {
        abilities[index].Cast(castData);
    }

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
            return;
        for (int i = 0; i < abilities.Length; i++)
        { 
            abilities[i].OnDrawGizmos();
        }

     }
}
