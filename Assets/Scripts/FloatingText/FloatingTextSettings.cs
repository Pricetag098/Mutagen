using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/FloatingTextSettings")]
public class FloatingTextSettings : ScriptableObject
{
    [Header("References")]
    public GameObject textPrefab;
    public Color[] colors;

    [Header("Stats")]
    public float textDeviation;
    public float baseTextSize;
    public float baseDuration;
    public float durationDamageMultiplier;

    [Header("Motion")]
    [Range(0f, 1f)]
    public float textSpread;
    public float motionSpeed;
    public float followStrength;
}
