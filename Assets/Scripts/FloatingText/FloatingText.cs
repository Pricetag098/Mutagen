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
    

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
        curDeviation = Random.Range(-deviation, deviation);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration)
            Hide();

        if (follow)
        {

            Vector3 dev = new Vector3(follow.transform.position.x + curDeviation, follow.transform.position.y + curDeviation, follow.transform.position.z + curDeviation);

            go.transform.position = Camera.main.WorldToScreenPoint(dev);

        }
        go.transform.position += motion * Time.deltaTime;
    }
}
