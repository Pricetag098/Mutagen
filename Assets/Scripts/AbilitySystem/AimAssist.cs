using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AimAssist
{
    [SerializeField] float maxRange = 100;
    [SerializeField] LayerMask aimAssistLayer  = 512;
    [SerializeField] float directionWeight = 1;
    [SerializeField] float distanceWeight = 1;
    [SerializeField] float priorityWeight = 1;
    [Range(-1, 1)]
    [SerializeField] float minDirectionValue = -1;
    [Range(0f, 1f)]
    [SerializeField] float assistWeight = 1;
    public Vector3 GetAssistedAimDir(Vector3 aimDir,Vector3 origin, float projectileSpeed)
    {
        if (assistWeight == 0)
            return aimDir;

        Collider[] colliders = Physics.OverlapSphere(origin, maxRange, aimAssistLayer);

        Vector3 bestTarget = aimDir;
        float bestVal = float.NegativeInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            AimAssistTarget target;
            if (colliders[i].TryGetComponent(out target))
            {


                Vector3 casterToTarget = target.transform.position - origin;
                casterToTarget.y = 0;

                float time = casterToTarget.magnitude / projectileSpeed;
                Vector3 newPos = target.transform.position + target.GetVelocity() * time;
                casterToTarget = newPos - origin;
                casterToTarget.y = 0;

                Vector3 casterToTargetNorm = casterToTarget.normalized;
                float directionValue = Vector3.Dot(aimDir, casterToTargetNorm) * directionWeight * target.size;
                if (directionValue < minDirectionValue)
                    continue;
                float val = directionValue + (1 / casterToTarget.magnitude) * distanceWeight + target.priority * priorityWeight;
                if (val > bestVal)
                {
                    bestVal = val;
                    bestTarget = casterToTargetNorm;
                }

            }
        }
        return Vector3.Lerp(aimDir, bestTarget, assistWeight).normalized;
    }

}
