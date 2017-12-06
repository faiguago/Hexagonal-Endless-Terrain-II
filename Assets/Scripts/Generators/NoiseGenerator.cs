using UnityEngine;
using Random = System.Random;

public static class NoiseGenerator
{

    public static float[] noise(Vector3[] vertices,
        Vector3 pos, ref NoiseVars nVars)
    {
        Random rnd = new Random(nVars.Seed);
        Vector2[] offsets = new Vector2[nVars.Octaves];

        float[] values = new float[vertices.Length];

        for (int i = 0; i < nVars.Octaves; i++)
        {
            offsets[i] = new Vector2(rnd.Next(-10000, 10000),
                rnd.Next(-10000, 10000));
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 point =
                new Vector2(vertices[i].x + pos.x + offsets[0].x,
                vertices[i].z + pos.z + offsets[0].y);

            float frequency = nVars.Frequency;

            float sum = Mathf.PerlinNoise(point.x * frequency / 10000,
                point.y * frequency / 10000);

            float amplitude = 1f;
            float range = 1f;

            for (int j = 1; j < nVars.Octaves; j++)
            {
                frequency *= nVars.Lacunarity;
                amplitude *= nVars.Persistence;
                range += amplitude;

                point =
                    new Vector2(vertices[i].x + pos.x + offsets[j].x,
                    vertices[i].z + pos.z + offsets[j].y);

                sum += Mathf.PerlinNoise(point.x * frequency / 10000,
                    point.y * frequency / 10000) * amplitude;
            }

            values[i] = sum / range;
        }

        return values;
    }

}
