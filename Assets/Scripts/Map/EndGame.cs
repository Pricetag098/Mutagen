using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public Credits credits;
    public GameObject ui;
    public void Quit()
    {
        ui.SetActive(false);
        credits.Open();
    }
}
