using System;
using UnityEngine;

[Serializable]
public struct TerrainVars
{
    [SerializeField] [Range(16, 145)] private int size;
    [SerializeField] private int scale;
    [SerializeField] private float height;
    [SerializeField] private int gridSize;
    [SerializeField] [Range(1, 16)] private int colLOD;

    public int Size { get { return size >= 16 ? (size / ColLOD) * ColLOD : 16; } }
    public int Scale { get { return scale >= 1 ? scale : 1; } }
    public float Height { get { return height; } }

    public int WorldSize { get { return Size * Scale; } }
    public int GridSize { get {
            return gridSize >= 1 ? gridSize : 1; } }
    private int ColLOD
    {
        get
        {
            if (colLOD <= 1)
            {
                return 1;
            }
            else
            {
                return (colLOD / 2) * 2;
            }
        }
    }
    public int ColSize { get { return Size / ColLOD; } }
    public int ColScale { get { return Scale * ColLOD; } }
    
    public Gradient gradient;
    public Material material;
}

[Serializable]
public struct NoiseVars
{
    [SerializeField] private float frequency;
    [Range(1, 8)]
    [SerializeField] private int octaves;
    [Range(1f, 4f)]
    [SerializeField] private float lacunarity;
    [Range(0f, 1f)]
    [SerializeField] private float persistence;
    [SerializeField] private int seed;

    public float Frequency { get { return frequency; } }
    public int Octaves { get { return octaves >= 1 ? octaves : 1; } }
    public float Lacunarity { get { return lacunarity >= 1f ? lacunarity : 1f; } }
    public float Persistence { get { return persistence; } }
    public int Seed { get { return seed; } }
}

public struct MeshData
{
    public Vector3[] vertices;
    public Vector3[] normals;
    public int[] triangles;
    public Color[] colors;

    public void RecalculateNormals()
    {
        normals = new Vector3[vertices.Length];

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 normal =
                CalculateNormal(vertices[triangles[i]],
                vertices[triangles[i + 1]],
                vertices[triangles[i + 2]]);

            normals[triangles[i]] += normal;
            normals[triangles[i + 1]] += normal;
            normals[triangles[i + 2]] += normal;
        }

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }
    }

    private Vector3 CalculateNormal(
        Vector3 A, Vector3 B, Vector3 C)
    {
        return Vector3.Cross(
            (B - A).normalized,
            (C - A).normalized);
    }

}