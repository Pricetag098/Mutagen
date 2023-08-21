using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    List<FloatingText> floatingTexts = new List<FloatingText>();

    private void FixedUpdate()
    {
        foreach(FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void Show(HitBox.TextData data) //change for specific purpose
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = data.damage.ToString();
        floatingText.txt.fontSize = data.fontSize;
        floatingText.txt.color = data.color;

        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(data.position);
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
