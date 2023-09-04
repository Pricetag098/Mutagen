using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField]Ability ability;
    Image icon;
    [SerializeField]Image cooldown;
    bool active;
    // Start is called before the first frame update
    void Awake()
    {
        icon = GetComponent<Image>();
        cooldown = transform.parent.GetChild(1).GetComponent<Image>();
    }

    public void SetAbility( Ability ability)
	{
        if(ability is null)
		{
            return;
		}
        active = true;
        this.ability = ability;
        icon.sprite = ability.icon;
	}

    // Update is called once per frame
    void Update()
    {
        if(active)
        cooldown.fillAmount = 1- ability.GetCoolDownPercent();
    }
}
