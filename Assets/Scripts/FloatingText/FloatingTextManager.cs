using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    public float textDeviation;

    List<FloatingText> floatingTexts = new List<FloatingText>();

    private void FixedUpdate()
    {
        foreach(FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void Show(HitBox.TextData data)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = data.damage < 99? data.damage.ToString().Substring(0, 2) : data.damage.ToString().Substring(0, 3);
        floatingText.txt.fontSize = data.fontSize;
        floatingText.txt.color = data.color;

        floatingText.follow = data.obj;
        floatingText.deviation = textDeviation;
        //floatingText.go.transform.position = Camera.main.WorldToScreenPoint(data.position);
        //floatingText.go.transform.position = new Vector3(floatingText.go.transform.position.x + Random.Range(-textDeviation, textDeviation) ,
        //    floatingText.go.transform.position.y + Random.Range(-textDeviation, textDeviation),
        //    floatingText.go.transform.position.z + Random.Range(-textDeviation, textDeviation));
        floatingText.motion = Vector3.up;
        floatingText.duration = data.duration;
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
