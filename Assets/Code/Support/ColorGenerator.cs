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

        var biomesLength = colorSettings.biomeColorSettings.biomes.Length;
        if (texture == null || texture.height != biomesLength) {
            texture = new Texture2D(
                textureResolution,
                biomesLength,
                TextureFormat.RGBA32, 
                false
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

    public float BiomePercentFromPoint(Vector3 point) {
        var heightPercent = (point.y + 1) / 2f;
        var biomeIndex = 0;
        var biomeLength = colorSettings.biomeColorSettings.biomes.Length;

        for (int i = 0; i < biomeLength; i++) {
            if (colorSettings.biomeColorSettings.biomes[i].startHeight < heightPercent) {
                biomeIndex = i;
            } else {
                break;
            }
        }

        return biomeIndex / Mathf.Max(
            1, 
            biomeLength - 1
        );
    }

    public void UpdateColors() {
        var colors = new Color[texture.width * texture.height];

        var colorIndex = 0;
        foreach (var biome in colorSettings.biomeColorSettings.biomes) {
            for (int i = 0; i < textureResolution; i++) {
                var gradientColor = biome.gradient.Evaluate(i / (textureResolution - 1f));
                var tintColor = biome.tint;

                colors[colorIndex] = gradientColor * (1 - biome.tintPercent) + tintColor * biome.tintPercent;

                colorIndex++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        colorSettings.planetMaterial.SetTexture(
            "_texture", 
            texture
        );
    }
}
