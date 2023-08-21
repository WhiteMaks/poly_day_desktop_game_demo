using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public FilterType filterType;
    public Vector3 center;
    public float strength = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    [Range(1, 8)]
    public int numOfLayers = 1;
    public float persistence = 0.5f;
    public float minValue;
    public float weightMultiplier = 0.8f;
}
