using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator {
    private MinMax elevation;
    private ShapeSettings shapeSettings;
    private NoiseFilter[] noiseFilters;

    public void UpdateSettings(ShapeSettings shapeSettings) {
        this.shapeSettings = shapeSettings;

        noiseFilters = new NoiseFilter[shapeSettings.noiseLayers.Length];
        elevation = new MinMax();

        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++) {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(
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

        evaluationNoise = shapeSettings.planetRadius * (1 + evaluationNoise);

        elevation.AddValue(evaluationNoise);
        
        return pointOnSphere * evaluationNoise;
    }

    public MinMax GetElevation() {
        return elevation;
    }
}
