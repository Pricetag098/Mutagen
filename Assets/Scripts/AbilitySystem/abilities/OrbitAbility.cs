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
    Timer timer;

    public List<GameObject> orbs = new List<GameObject>();
    [SerializeField] Vector3 offset;
    [SerializeField] float orbitSpeed;
    [SerializeField] float radius;
    [SerializeField] float flySpeed;
    [SerializeField] float midPointOffset;
    [SerializeField] float range;
    [SerializeField] float damage;
    [SerializeField] float damageRange;
    [SerializeField] AimAssist aimAssist;
    [SerializeField] VfxSpawnRequest spawnFx;
    protected override void OnEquip()
    {
        caster.abilities[0].OnCast += Fire;
        pooler = new GameObject().AddComponent<ObjectPooler>();
        pooler.CreatePool(prefab, 5 * maxCharges);
        timer = new Timer(cooldown);
        
    }

	protected override void OnUnEquip(Ability replacement)
	{
        orbs.Clear();
	}

	void Fire(CastData data)
    {
        
        if(orbs.Count > 0)
        {
            GameObject orb = orbs[0];
            orbs.RemoveAt(0);

            Vector3 aimAssistDir = aimAssist.GetAssistedAimDir(data.aimDirection, data.origin, flySpeed);

            Vector3 endPoint = data.origin + aimAssistDir * range;
            Vector3 midPoint = data.origin + aimAssistDir * midPointOffset;
            float tempdmg = damage;
            tempdmg += Random.Range(-damageRange, damageRange);
            if (tempdmg < 0)
                tempdmg = 0;
            orb.GetComponent<OrbitProjectile>().Shoot(endPoint, midPoint, this, flySpeed,CreateDamageData(tempdmg));

        }
    }

    public override void OnTick()
    {
        timer.Tick();
        for(int i = orbs.Count-1; i >=0 ; i--) 
        {
            GameObject orb = orbs[i];
            if (!orb.activeSelf)
            {
                orbs.RemoveAt(i);
                continue;
            }
            float waveLength = orb.GetComponent<OrbitProjectile>().waveLength;
            orb.GetComponent<OrbitProjectile>().damage = CreateDamageData(damage);
            orb.transform.position = caster.transform.position + offset + new Vector3(Mathf.Sin((Time.time + waveLength) * orbitSpeed),0,Mathf.Cos((Time.time + waveLength) * orbitSpeed)) * radius;
            
        }
    }
    protected override void DoCast(CastData data)
    {
        if(timer.complete)
        {
            int count = maxCharges - orbs.Count;
            Debug.Log(count);
            for (int i = 0; i < count; i++)
            {
                GameObject go = pooler.Spawn();
                float waveLength = (i / (float)maxCharges) * 2 * Mathf.PI;
                go.GetComponent<OrbitProjectile>().waveLength = waveLength;
                go.transform.position = caster.transform.position + offset + new Vector3(Mathf.Sin((Time.time + waveLength) * orbitSpeed), 0, Mathf.Cos((Time.time + waveLength) * orbitSpeed)) * radius;
                spawnFx.Play(go.transform.position - offset*.99f,Vector3.up);
                go.GetComponent<OrbitProjectile>().alive = true;

                orbs.Add(go);
            }
            timer.Reset();
            FinishCast();
        }
    }
    public override float GetCoolDownPercent()
    {
        return timer.Progress;
    }
}
