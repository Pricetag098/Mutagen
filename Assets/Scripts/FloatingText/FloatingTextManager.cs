using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public ObjectPooler pooler;
    public FloatingTextSettings settings;
    public int poolSize;
    [HideInInspector] public List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Start()
    {
        FindObjectOfType<PlayerAbilityCaster>().GetComponent<FloatingTextTarget>().textManager = this;

        pooler = GetComponent<ObjectPooler>();
        pooler.CreatePool(settings.textPrefab, poolSize);

        for(int i = 0; i < poolSize; i++)
        {
            FloatingText txt = GetFloatingText();
            txt.Hide();
        }
    }

    private void FixedUpdate()
    {
        foreach(FloatingText txt in floatingTexts)
        {
            txt.UpdateFloatingText();          
        }
    }

    public void Show(DamageData data)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.damage = data.damage;


        floatingText.txt.text = ((int)data.damage).ToString();
        floatingText.txt.fontSize = settings.baseTextSize + (data.damage / 10);
        floatingText.txt.color = settings.colors[(int)data.type];

        //motion
        floatingText.follow = data.target;
        floatingText.deviation = settings.textDeviation;
        floatingText.motion = new Vector3((Random.Range(-settings.textSpread, settings.textSpread)), 1, 0);
        floatingText.followStrength = settings.followStrength;
        floatingText.motionSpeed = settings.motionSpeed;

        floatingText.duration = settings.baseDuration;
        floatingText.Show();
    }

    FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = pooler.Spawn();
            txt.go.transform.SetParent(transform);
            txt.txt = txt.go.GetComponent<TextMeshProUGUI>();
            txt.manager = this;
            floatingTexts.Add(txt);
        }

        return txt;
    }
}
