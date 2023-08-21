using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNoiseFilter : NoiseFilter
{
    protected NoiseSettings noiseSettings;
    protected Noise noise = new Noise();

    protected BaseNoiseFilter(NoiseSettings noiseSettings) {
        this.noiseSettings = noiseSettings;
    }

    public abstract float EvaluateNoise(Vector3 point);
}
