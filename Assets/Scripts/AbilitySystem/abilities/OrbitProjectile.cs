using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitProjectile : MonoBehaviour
{
    Vector3 start,mid, end;
    float timer, maxTimer;
    OrbitAbility ability;
    bool flying = false;
    public DamageData damage;
    public bool alive;
    [SerializeField] VfxSpawnRequest vfx;
    public float waveLength;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PooledObject>().OnDespawn += Despawn;
    }

    void Despawn()
    {
        flying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flying)
        {
            timer += Time.deltaTime;
            transform.position = QaudraticLerp(start, mid, end, timer / maxTimer);
            if(timer > maxTimer)
            {
                GetComponent<PooledObject>().Despawn();
            }
        }
    }

    public void Shoot(Vector3 endPoint, Vector3 midPoint,OrbitAbility ab,float speed,DamageData dmg)
    {
        start = transform.position;
        mid = midPoint;
        end = endPoint;
        maxTimer = Vector3.Distance(start,endPoint)/ speed;
        timer = 0;
        flying = true;
        damage = dmg;
        ability = ab;
    }
    Vector3 QaudraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!alive)
            return;
        alive = false;
        HitBox hb;
        if (collision.gameObject.TryGetComponent(out hb))
        {
            hb.OnHit(damage);
        }
        vfx.Play(transform.position, Vector3.up);
        GetComponent<PooledObject>().Despawn();
        
    }
}
