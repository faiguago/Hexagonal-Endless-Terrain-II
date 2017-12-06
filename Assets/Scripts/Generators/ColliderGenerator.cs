using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderGenerator
{
    public static MeshData Generate(Datas datas, Vector3 pos)
    {
        MeshData mesh = HexMeshGenerator.GenerateMesh(
            datas.tVars.ColSize, datas.tVars.ColScale);

        AddNoise(ref datas.tVars, ref datas.nVars, ref mesh, pos);

        return mesh;
    }

    private static void AddNoise(ref TerrainVars tVars,
        ref NoiseVars nVars, ref MeshData mesh, Vector3 pos)
    {
        Vector3[] vertices = mesh.vertices;
        float height = tVars.Height;

        float[] noise = NoiseGenerator.noise(vertices, pos, ref nVars);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 currentVertex = vertices[i];
            vertices[i].y = (noise[i] * 2 - 1) * height;
        }

        mesh.vertices = vertices;
    }
}
