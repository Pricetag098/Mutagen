using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "On Hit/StatusEffect")]
public class OnHitApplyStatusEffect : OnHitEffect
{
    [SerializeField] StatusEffect effect;
    public override void OnHit(HitBox hitBox, Vector3 direction)
    {
        hitBox.health.AddStatusEffect(effect);
    }
}
