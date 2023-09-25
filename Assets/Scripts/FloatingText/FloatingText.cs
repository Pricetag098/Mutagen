using UnityEngine;
using TMPro;

public class FloatingText
{
    //refs
    public GameObject go;
    public GameObject follow;
    public TextMeshProUGUI txt;
    public FloatingTextManager manager;
    Health followHealth;

    //stats
    public bool active;
    public Vector3 motion;
    public float duration;
    public float lastShown;
    float curDeviation;
    public float deviation;
    public float motionSpeed;
    public float followStrength;
    public float damage;
    public bool canMerge = true;
    
    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(true);
        Vector3 curDev = new Vector3(Random.Range(-deviation, deviation), Random.Range(-deviation, deviation), 0);       
        go.transform.position = Camera.main.WorldToScreenPoint(follow.transform.position + curDev);

        followHealth = follow.GetComponent<Health>();
    }

    public void Hide()
    {
        active = false;
        go.SetActive(false);
        manager.pooler.Despawn(go.GetComponent<PooledObject>());

    }

    public void Merge(FloatingText other)
    {
        damage += other.damage;

        float textDivide = damage % 10;

        txt.text = ((int)damage).ToString();

        lastShown = Time.time - manager.settings.addedMergeDuration;
        other.Hide();
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration)
        {
            Hide();
            return;
        }



        if (!followHealth.dead)
        {
            Vector3 dir = (Camera.main.WorldToScreenPoint(follow.transform.position) - go.transform.position);
            float dist = Vector3.Distance(dir , follow.transform.position);
            if (dist > manager.settings.followDist)
                go.transform.position +=
                    ((Camera.main.WorldToScreenPoint(follow.transform.position)) - go.transform.position).normalized * followStrength;
            else
                go.transform.position +=
                    ((Camera.main.WorldToScreenPoint(follow.transform.position)) - go.transform.position).normalized * (followStrength / 3);
            
            go.transform.position += motion * motionSpeed * Time.deltaTime;
        }
        else
            Hide();



        foreach (FloatingText othertxt in manager.floatingTexts)
        {
            if (!othertxt.active)
                return;



            if (this != othertxt && follow == othertxt.follow)
            {
                if (Vector3.Distance(go.transform.position, othertxt.go.transform.position) < manager.settings.mergeDistance)
                {
                    Merge(othertxt);
                }
            }
        }
    }
}