using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator shapeGenerator;
    private Mesh mesh;
    private Vector3 axisZ;
    private Vector3 axisX;
    private Vector3 axisY;

    private int resolution;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 axisZ) {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.axisZ = axisZ;

        axisX = new Vector3(axisZ.y, axisZ.z, axisZ.x);
        axisY = Vector3.Cross(axisZ, axisX);
    }

    //resolution = 4
    //
    //      . . . .
    //      . . . .
    //      . . . .
    //      . . . .
    //
    //if you circle the dots together, you get a matrix 3x3
    //
    //      |1|2|3|
    //      |2| | |
    //      |3| | |
    //      
    //vertices = resolution^2 = 16
    //16 points in total, 4 points for each square
    //
    //each square has 2 triangles
    //
    //      |\|\|\|
    //      |\|\|\|
    //      |\|\|\|
    //
    //in one line, the number of triangles will be equal to 
    //trianglesInLine = (resolution - 1) * 2
    //
    //each triangle has three points
    //
    //      .
    //      . .
    //
    //triangles = trianglesInLine^2 * 3
    public void ConstructMesh() {
        var vertices = new Vector3[resolution * resolution];

        var trianglesInLine = (resolution - 1) * 2;
        var triangles = new int[trianglesInLine * trianglesInLine * 3];
        var triangleIndex = 0;
        
        var uv = mesh.uv;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                var index = x + y * resolution;
                var percent = new Vector2(x, y) / (resolution - 1);
                var pointOnCube = axisZ + (percent.x - 0.5f) * 2 * axisX + (percent.y - 0.5f) * 2 * axisY;
                var pointOnSphere = pointOnCube.normalized;
                vertices[index] = shapeGenerator.CalculatePointOnPlanet(pointOnSphere);

                if (x != resolution - 1 && y != resolution - 1) {
                    triangles[triangleIndex] = index;
                    triangles[triangleIndex + 1] = index + resolution + 1;
                    triangles[triangleIndex + 2] = index + resolution;

                    triangles[triangleIndex + 3] = index;
                    triangles[triangleIndex + 4] = index + 1;
                    triangles[triangleIndex + 5] = index + resolution + 1;

                    triangleIndex += 6;
                }
            }
        }

        updateMesh(
            vertices,
            triangles,
            uv
        );
    }

    private void updateMesh(Vector3[] vertices, int[] triangles, Vector2[] uv) {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;
    }

    public void UpdateUVs(ColorGenerator colorGenerator) {
        var uv = new Vector2[resolution * resolution];

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                var index = x + y * resolution;
                var percent = new Vector2(x, y) / (resolution - 1);
                var pointOnCube = axisZ + (percent.x - 0.5f) * 2 * axisX + (percent.y - 0.5f) * 2 * axisY;
                var pointOnSphere = pointOnCube.normalized;

                uv[index] = new Vector2(
                    colorGenerator.BiomePercentFromPoint(pointOnSphere), 
                    0
                );
            }
        }

        mesh.uv = uv;
    }
}
