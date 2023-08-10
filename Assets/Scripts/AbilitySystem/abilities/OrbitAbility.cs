using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Orbit")]
public class OrbitAbility : Ability
{
    [SerializeField] int maxCharges = 3;

    [SerializeField] GameObject prefab;
    ObjectPooler pooler;

    [SerializeField] float cooldown;
    float timer;

    public List<GameObject> orbs = new List<GameObject>();
    [SerializeField] Vector3 offset;
    [SerializeField] float orbitSpeed;
    [SerializeField] float radius;
    [SerializeField] float flySpeed;
    [SerializeField] float midPointOffset;
    [SerializeField] float range;
    [SerializeField] float damage;
    [SerializeField] AimAssist aimAssist;
    protected override void OnEquip()
    {
        caster.abilities[1].OnCast += Fire;
        pooler = new GameObject().AddComponent<ObjectPooler>();
        pooler.CreatePool(prefab, 5 * maxCharges);
    }

    void Fire(CastData data)
    {
        
        if(orbs.Count >0)
        {
            GameObject orb = orbs[0];
            orbs.RemoveAt(0);

            Vector3 aimAssistDir = aimAssist.GetAssistedAimDir(data.aimDirection, data.origin, flySpeed);

            Vector3 endPoint = data.origin + aimAssistDir * range;
            Vector3 midPoint = data.origin + aimAssistDir * midPointOffset;
            orb.GetComponent<OrbitProjectile>().Shoot(endPoint, midPoint, this, flySpeed,damage);

        }
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        for(int i = orbs.Count-1; i >=0 ; i--) 
        {
            GameObject orb = orbs[i];
            if (!orb.activeSelf)
            {
                orbs.RemoveAt(i);
                continue;
            }
            float waveLength = (i/ (float) orbs.Count) * 2* Mathf.PI;
            
            orb.transform.position = caster.transform.position + offset + new Vector3(Mathf.Sin((Time.time + waveLength) * orbitSpeed),0,Mathf.Cos((Time.time + waveLength) * orbitSpeed)) * radius;
        }
    }
    protected override void DoCast(CastData data)
    {
        if(timer >= cooldown)
        {
            int count = maxCharges - orbs.Count;
            for (int i = 0; i < count; i++)
            {
                orbs.Add(pooler.Spawn());
            }
            timer = 0;
        }
    }
    public override float GetCoolDownPercent()
    {
        return Mathf.Clamp01(timer / cooldown);
    }
}
