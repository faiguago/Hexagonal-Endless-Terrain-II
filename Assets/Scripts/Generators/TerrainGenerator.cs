using UnityEngine;

public static class TerrainGenerator
{

    public static MeshData Generate(Datas datas, Vector3 pos)
    {
        MeshData mesh = HexMeshGenerator.GenerateMesh(datas.tVars.Size, datas.tVars.Scale);

        AddNoise(ref datas.tVars, ref datas.nVars, ref mesh, pos);

        return mesh;
    }

    private static void AddNoise(ref TerrainVars tVars,
        ref NoiseVars nVars, ref MeshData mesh, Vector3 pos)
    {
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        Gradient gradient = tVars.gradient;
        float height = tVars.Height;

        float[] noise = NoiseGenerator.noise(vertices, pos, ref nVars);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 currentVertex = vertices[i];
            colors[i] = gradient.Evaluate(noise[i]);
            vertices[i].y = (noise[i] * 2 - 1) * height; 
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

}
