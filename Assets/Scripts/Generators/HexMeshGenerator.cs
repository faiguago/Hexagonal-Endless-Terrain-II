using System.Collections.Generic;
using UnityEngine;

public static class HexMeshGenerator
{

    public static MeshData GenerateMesh(int size, int scale)
    {
        MeshData mesh = new MeshData();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        CreateVertices(size, scale, vertices);
        CreateTriangles(size, scale, triangles);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    private static void CreateVertices(int size, int scale,
        List<Vector3> vertices)
    {
        for (int z = 0; z <= size; z++)
        {
            int xSize = size + z;

            for (int x = 0; x <= xSize; x++)
            {
                vertices.Add(new Vector3(x - Hex.cos60 * z - size / 2f,
                    0, (z - size) * Hex.sin60) * scale);
            }
        }
        for (int z = size + 1, i = 0; z <= 2 * size; z++, i++)
        {
            int xSize = 2 * size - i - 1;

            for (int x = 0; x <= xSize; x++)
            {
                vertices.Add(new Vector3(x - (size - 1 - i) * Hex.cos60 - size / 2f,
                    0, (z - size) * Hex.sin60) * scale);
            }
        }
    }

    private static void CreateTriangles(int size, int scale,
        List<int> triangles)
    {
        int i = 0;

        for (int z = 0; z < size; z++, i++)
        {
            int xSize = size + z;

            for (int x = 0; x < xSize; x++, i++)
            {
                triangles.Add(i);
                triangles.Add(i + xSize + 1);
                triangles.Add(i + xSize + 2);

                triangles.Add(i);
                triangles.Add(i + xSize + 2);
                triangles.Add(i + 1);

                if (x == xSize - 1)
                {
                    triangles.Add(i + 1);
                    triangles.Add(i + xSize + 2);
                    triangles.Add(i + xSize + 3);
                }
            }
        }

        for (int z = 0; z < size; z++, i++)
        {
            int xSize = 2 * size - z;

            for (int x = 0; x < xSize; x++, i++)
            {
                triangles.Add(i);
                triangles.Add(i + xSize + 1);
                triangles.Add(i + 1);

                if (x != xSize - 1)
                {
                    triangles.Add(i + 1);
                    triangles.Add(i + xSize + 1);
                    triangles.Add(i + xSize + 2);
                }
            }
        }
    }
	
}
