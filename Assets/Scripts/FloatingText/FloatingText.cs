using UnityEngine;
using TMPro;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public GameObject follow;
    public TextMeshProUGUI txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;
    float curDeviation;
    public float deviation;
    public float motionSpeed;
    public float followStrength;
    public FloatingTextManager manager;
    

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);

        Vector3 curDev = new Vector3(Random.Range(-deviation, deviation), Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));       
        go.transform.position = Camera.main.WorldToScreenPoint(follow.transform.position + curDev);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (follow)
        {
            go.transform.position += ((Camera.main.WorldToScreenPoint(follow.transform.position)) - go.transform.position).normalized * followStrength;


        }

        go.transform.position += motion * motionSpeed * Time.deltaTime;

    }
}
