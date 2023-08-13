using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilitySelector : MonoBehaviour
{
    Ability heldAbility;
    Button[] buttons;
    [SerializeField] PlayerAbilityCaster abilityCaster;
    [SerializeField] Image heldIcon;
<<<<<<< HEAD
    public bool open;
=======
>>>>>>> Art
    // Start is called before the first frame update
    void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    

    public void Select(int index)
	{
        abilityCaster.caster.SetAbility(heldAbility,index);
        gameObject.SetActive(false);
        Time.timeScale = 1;
<<<<<<< HEAD
        open = false;
=======
        
>>>>>>> Art
    }

    public void Open( Ability ability)
	{
        Time.timeScale = 0;
        gameObject.SetActive(true);
        heldAbility = ability;
        for(int i = 0; i < buttons.Length; i++)
		{
            Button button = buttons[i];
            button.interactable = ability.slotMask.HasFlag((Ability.SlotMask)Mathf.Pow(2, i));
            button.image.sprite = abilityCaster.caster.abilities[i].icon;
        }
        heldIcon.sprite = ability.icon;
<<<<<<< HEAD
        open = true;
=======

>>>>>>> Art
        
	}
}
