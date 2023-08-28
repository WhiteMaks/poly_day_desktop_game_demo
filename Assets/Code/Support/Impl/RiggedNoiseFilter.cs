using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggedNoiseFilter : BaseNoiseFilter
{
    public RiggedNoiseFilter(NoiseSettings noiseSettings) : base(noiseSettings) {

    }

    public override float EvaluateNoise(Vector3 point) {
        var noiseValue = 0f;
        var frequency = noiseSettings.baseRoughness;
        var amplitude = 1f;
        var weight = 1f;

        for (int i = 0; i < noiseSettings.numOfLayers; i++) {
            var noiseEvaluation = 1 - Mathf.Abs(
                noise.Evaluate(point * frequency + noiseSettings.center)
            );

            noiseEvaluation *= noiseEvaluation;
            noiseEvaluation *= weight;

            weight = Mathf.Clamp01(noiseEvaluation * noiseSettings.weightMultiplier);

            noiseValue += noiseEvaluation * amplitude;

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
