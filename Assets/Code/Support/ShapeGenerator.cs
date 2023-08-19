using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator {
    private ShapeSettings shapeSettings;
    private NoiseFilter[] noiseFilters;

    public ShapeGenerator(ShapeSettings shapeSettings) {
        this.shapeSettings = shapeSettings;

        noiseFilters = new NoiseFilter[shapeSettings.noiseLayers.Length];

        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++) {
            noiseFilters[i] = new NoiseFilter(
                shapeSettings.noiseLayers[i].noiseSettings
            );
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnSphere) {
        var firstLayerValue = 0f;
        var evaluationNoise = 0f;

        if (noiseFilters.Length > 0) {
            firstLayerValue = noiseFilters[0].EvaluateNoise(pointOnSphere);

            if (shapeSettings.noiseLayers[0].enabled) {
                evaluationNoise = firstLayerValue;
            }
        }

        for (int i = 0; i < noiseFilters.Length; i++) {
            if (shapeSettings.noiseLayers[i].enabled) {
                var mask = shapeSettings.noiseLayers[i].useFirstLayerMask ? 
                    firstLayerValue : 1;

                evaluationNoise += noiseFilters[i].EvaluateNoise(pointOnSphere) * mask;
            }
        }

        return pointOnSphere * shapeSettings.planetRadius * (1 + evaluationNoise);
    }
}
