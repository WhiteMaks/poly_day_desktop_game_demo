using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Biome
{
    public Gradient gradient;
    public Color tint;

    [Range(0, 1)]
    public float startHeight;
    [Range(0, 1)]
    public float tintPercent;
}
