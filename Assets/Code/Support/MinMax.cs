using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    private float min;
    private float max;

    public MinMax() {
        min = float.MaxValue;
        max = float.MinValue;
    }

    public void AddValue(float value) {
        if (value > max) {
            max = value;
        }

        if (value < min) {
            min = value;
        }
    }

    public float GetMax() {
        return max;
    }

    public float GetMin() {
        return min;
    }
}
