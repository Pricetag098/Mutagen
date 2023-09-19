using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StatusEffects/Slow")]
public class SlowStatus : StatusEffect
{
    public float slowPercent = 1;
    public float duration;

    Timer timer;
    protected override void OnAdd()
    {
        timer = new Timer(duration, false);
        if (health.TryGetComponent(out Enemy enemy))
        {
            enemy.movementMultiplier -= slowPercent;
        }
        if(health.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.speedMulti -= slowPercent;
        }
    }
    public override void Tick()
    {
        if (timer.complete)
            health.RemoveStatusEffect(this);

    }
    protected override void OnRemove()
    {
        if (health.TryGetComponent(out Enemy enemy))
        {
            enemy.movementMultiplier += slowPercent;
        }
        if (health.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.speedMulti += slowPercent;
        }
    }
    public override void Combine(StatusEffect effect)
    {
        SlowStatus slowStatus = effect as SlowStatus;

        if (health.TryGetComponent(out Enemy enemy))
        {
            enemy.movementMultiplier -= slowStatus.slowPercent;
        }
        if (health.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.speedMulti -= slowStatus.slowPercent;
        }
        slowPercent += slowStatus.slowPercent;
        timer.maxTime += slowStatus.duration;
    }
}
