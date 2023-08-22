using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ability;

public class HitBox : MonoBehaviour
{
    public float multi = 1;
    public FloatingTextManager textManager;
    public Color critColor;
    public float textDuration;


    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        health = GetComponentInParent<Health>();

        textManager = FindObjectOfType<FloatingTextManager>(); //will replace later
    }
    public void OnHit(DamageData data) //changed ability to atk ability if layer is added
    {
        data.damage *= multi;
        health.TakeDmg(data);

        //textManager.Show(CreateTextData(ability));
        //textManager.Show(CreateTextData(dmg)); //temp until layer is added
    }


    public struct TextData
    {
        public float damage;
        public float fontSize; //will adjust depending on damage
        public Color color; //will be equal to ability element
        public Vector3 position; //will be displayed on character hit
        public float duration; //changable
    }

    public TextData CreateTextData(float tempdmg) //changed ability to atk ability if layer is added
    {
        TextData data = new TextData();
        //float dmg = ability.damage * multi;
        float tempDmg = tempdmg * multi;
        data.damage = tempDmg;
        data.fontSize = 15 + (tempDmg / 10);
        data.color = Color.red; //will change to depend on element
        data.position = transform.position;
        data.duration = textDuration;
        return data;
    }
}
