using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/FloatingTextSettings")]
public class FloatingTextSettings : ScriptableObject
{
    public GameObject textPrefab;
    [Header("Stats")]
    public float textDeviation;
    public float baseTextSize;
    public float baseDuration;
    [Range(0f, 1f)]
    public float textSpread;
    public float motionSpeed;
    public float durationDamageMultiplier;
    public Color[] colors;
    public float followStrength;


}
