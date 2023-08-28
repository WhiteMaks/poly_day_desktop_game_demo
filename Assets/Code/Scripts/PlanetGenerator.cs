using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;
    private ShapeGenerator shapeGenerator;
    private ColorGenerator colorGenerator;
    private bool shapeSettingsIsVisible;
    private bool colorSettingsIsVisible;

    [Range(2, 256)]
    public int resolution = 10;
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public bool autoUpdate = true;

    public PlanetGenerator() {
        shapeGenerator = new ShapeGenerator();
        colorGenerator = new ColorGenerator();
    }

    public void OnColorSettingsUpdated() {
        if (autoUpdate) {
            Initialize();
            GenerateColors();
        }
    }

    public void OnShapeSettingsUpdated() {
        if (autoUpdate) {
            Initialize();
            GenerateMesh();
        }
    }

    public void Generate() {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public ref bool ShapeSettingsIsVisible() {
        return ref shapeSettingsIsVisible;
    }

    public ref bool ColorSettingsIsVisible() {
        return ref colorSettingsIsVisible;
    }

    private void Initialize() {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0) {
            meshFilters = new MeshFilter[6];
        }
       
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = {
            Vector3.up,
            Vector3.down, 
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < 6; i++) {
            if (meshFilters[i] == null) {
                var meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;
                meshObject.AddComponent<MeshRenderer>();

                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(
                shapeGenerator,
                meshFilters[i].sharedMesh, 
                resolution, 
                directions[i]
            );
        }
    }

    private void GenerateMesh() {
        foreach (var terrainFace in terrainFaces) {
            terrainFace.ConstructMesh();
        }

        colorGenerator.UpdateElevation(shapeGenerator.GetElevation());
    }

    private void GenerateColors() {
        colorGenerator.UpdateColors();

        foreach (var terrainFace in terrainFaces) {
            terrainFace.UpdateUVs(colorGenerator);
        }
    }
}
