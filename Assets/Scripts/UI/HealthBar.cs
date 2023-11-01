using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour
{
    public Health health;
    public Image image;
    public TMP_Text text;
    

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = health.health/health.maxHealth;
        text.text = (int)health.health + " | " + health.maxHealth;
    }
}
