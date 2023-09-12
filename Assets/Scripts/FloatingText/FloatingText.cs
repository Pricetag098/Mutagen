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
    public float damage;
    public bool canMerge = true;
    

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
        canMerge = true;
        Vector3 curDev = new Vector3(Random.Range(-deviation, deviation), Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));       
        go.transform.position = Camera.main.WorldToScreenPoint(follow.transform.position + curDev);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(false);
        manager.pooler.Despawn(go.GetComponent<PooledObject>());
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if(Time.time - lastShown > duration)
        {
            Hide();
        }

        //foreach (FloatingText text in manager.floatingTexts)
        //{
        //    if (text.active && text.follow == follow)
        //    {
        //        if (text.canMerge)
        //        {
        //            if (Vector3.Distance(go.transform.position, text.go.transform.position) < 0.01f)
        //            {
        //                damage += text.damage;
        //                if (damage <= 9)
        //                    txt.text = damage.ToString().Substring(0, 1);
        //                else if (damage < 100)
        //                    txt.text = damage.ToString().Substring(0, 2);
        //                else
        //                    txt.text = damage.ToString().Substring(0, 3);

        //                canMerge = false;
        //                text.Hide();
        //            }
        //        }
        //    }
        //}

        if (follow)
        {
            go.transform.position += ((Camera.main.WorldToScreenPoint(follow.transform.position)) - go.transform.position).normalized * followStrength;
        }
        go.transform.position += motion * motionSpeed * Time.deltaTime;
    }
}
