using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public static class NoiseFilterFactory
{

    public static NoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings) {
        switch (noiseSettings.filterType) {
            case FilterType.SIMPLE: {
                return new SimpleNoiseFilter(noiseSettings);
            }
            case FilterType.RIDGED: {
                return new RiggedNoiseFilter(noiseSettings);
            }
            default: {
                return null;
            }
        }
    }

}
