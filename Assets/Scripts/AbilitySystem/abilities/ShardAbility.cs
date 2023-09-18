using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shard")]
public class ShardAbility : Ability
{
    [SerializeField] float coolDown;
    [SerializeField] GameObject prefab;
    [SerializeField] float spawnDistance;
    Timer timer;
    ObjectPooler pooler;
    protected override void OnEquip()
    {
        timer = new Timer(coolDown);
        pooler = new GameObject().AddComponent<ObjectPooler>();
        pooler.CreatePool(prefab, 10);
    }
    protected override void DoCast(CastData data)
    {
        if (timer.complete)
        {
            timer.Reset();
            Vector3 spawnPos = data.origin + data.aimDirection * spawnDistance;
            GameObject shard = pooler.Spawn();
            shard.transform.position = spawnPos;
            if (shard.TryGetComponent(out Health health))
                health.Respawn();
        }
    }

    public override void Tick()
    {
        timer.Tick();
    }
}
