using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private ColorSettings colorSettings;
    private Texture2D texture;
    private const int textureResolution = 50;

    public void UpdateSettings(ColorSettings colorSettings) {
        this.colorSettings = colorSettings;

        if (texture == null) {
            texture = new Texture2D(
                textureResolution,
                1
            );
        }
    }

    public void UpdateElevation(MinMax elevation) {
        colorSettings.planetMaterial.SetVector(
            "_elevation", 
            new Vector2(
                elevation.GetMin(), 
                elevation.GetMax()
            )
        );
    }

    public void UpdateColors() {
        var colors = new Color[textureResolution];

        for (int i = 0; i < textureResolution; i++) {
            colors[i] = colorSettings.gradient.Evaluate(i / (textureResolution - 1f));
        }

        texture.SetPixels(colors);
        texture.Apply();

        colorSettings.planetMaterial.SetTexture(
            "_texture", 
            texture
        );
    }
}
