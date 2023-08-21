using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : BaseNoiseFilter
{
    public SimpleNoiseFilter(NoiseSettings noiseSettings) : base(noiseSettings) {
        this.noiseSettings = noiseSettings;
    }

    public override float EvaluateNoise(Vector3 point) {
        var noiseValue = 0f;
        var frequency = noiseSettings.baseRoughness;
        var amplitude = 1f;

        for (int i = 0; i < noiseSettings.numOfLayers; i++) {
            var noiseEvaluation = noise.Evaluate(point * frequency + noiseSettings.center);
            noiseValue += (noiseEvaluation + 1) * 0.5f * amplitude;

            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(
            0, 
            noiseValue - noiseSettings.minValue
        );

        return noiseValue * noiseSettings.strength;
    }
}
