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
    public GameObject textPrefab;
    [Header("Stats")]
    public float textDeviation;
    public float baseTextSize;
    public float baseDuration;
    public float durationDamageMultiplier;
    public Color[] colors;

    List<FloatingText> floatingTexts = new List<FloatingText>();

    private void FixedUpdate()
    {
        foreach(FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void Show(DamageData data)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = data.damage < 99? data.damage.ToString().Substring(0, 2) : data.damage.ToString().Substring(0, 3);
        floatingText.txt.fontSize = baseTextSize + (data.damage / 10);
        floatingText.txt.color = colors[(int)data.type];

        floatingText.follow = data.target;
        floatingText.deviation = textDeviation;

        floatingText.motion = Vector3.up;
        floatingText.duration = baseDuration;// + (data.damage / durationDamageMultiplier);
        floatingText.Show();
    }

    FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if(txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<TextMeshProUGUI>();

            floatingTexts.Add(txt);
        }

        return txt;
    }
}
