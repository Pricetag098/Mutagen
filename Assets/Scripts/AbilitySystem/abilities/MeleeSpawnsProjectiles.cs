using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/MeleeProjectiles")]
public class MeleeSpawnsProjectiles : MeleeAttackAbility
{
    [SerializeField] GameObject prefab;
    [SerializeField] float projectileSpeed = 1;
    [SerializeField] float projectileDamage = 1;
    ObjectPooler pooler;
    [SerializeField] AimAssist aimAssist;
    protected override void OnEquip()
    {
        base.OnEquip();

        pooler = new GameObject().AddComponent<ObjectPooler>();
        pooler.CreatePool(prefab, 10);
    }

    protected override void OnSwing(Vector3 origin, Vector3 direction)
    {
        direction = aimAssist.GetAssistedAimDir(direction, origin, projectileSpeed);
        pooler.Spawn().GetComponent<Projectile>().Launch(origin, direction * projectileSpeed, CreateDamageData(projectileDamage), hitEffects);
    }
}
