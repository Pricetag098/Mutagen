using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Rendering.FilterWindow;
//using System.Drawing;

public class FloatingTextManager : MonoBehaviour
{
    [Header("References")]
    public GameObject textContainer;
    public ObjectPooler pooler;
    public FloatingTextSettings settings;
    public int poolSize;
    List<FloatingText> floatingTexts = new List<FloatingText>(5);

    private void Start()
    {
        pooler = new GameObject().AddComponent<ObjectPooler>();
        pooler.CreatePool(settings.textPrefab, poolSize);
    }

    private void FixedUpdate()
    {
        foreach(FloatingText txt in floatingTexts)
        {
            txt.UpdateFloatingText();

            if(Time.time - txt.lastShown > txt.duration)
            {
                txt.active = false;
                pooler.Despawn(txt.go.GetComponent<PooledObject>());
            }
        }
    }

    public void Show(DamageData data)
    {
        FloatingText floatingText = GetFloatingText();
        //text
        if(data.damage <= 9)
            floatingText.txt.text =  data.damage.ToString().Substring(0, 1);
        else if(data.damage <= 99)
            floatingText.txt.text = data.damage.ToString().Substring(0, 2);
        else
            floatingText.txt.text = data.damage.ToString().Substring(0, 3);
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
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<TextMeshProUGUI>();
            txt.manager = this;
            floatingTexts.Add(txt);
        }

        return txt;
    }
}
