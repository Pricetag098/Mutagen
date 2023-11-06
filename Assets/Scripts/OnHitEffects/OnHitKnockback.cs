using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "On Hit/Knockback")]
public class OnHitKnockback : OnHitEffect
{
    [SerializeField] float force;

    public override void OnHit(HitBox hitBox, Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        Enemy enemy;
        if(hitBox.health.TryGetComponent(out enemy))
        {
            enemy.KnockBack(direction * force);
            
        }
        Rigidbody rb;
        if(hitBox.health.TryGetComponent(out rb))
        {
            rb.AddForce(direction * force,ForceMode.VelocityChange);
        }
    }
}
