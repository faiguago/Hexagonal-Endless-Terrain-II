using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),
    typeof(MeshRenderer))]
public class ChunkGenerator : MonoBehaviour
{
    public bool dummyVar;
    public bool showVertices;

    public Datas datas;
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private MeshData meshData;
    private Queue<Action> actionsToDo
        = new Queue<Action>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Generate(
        float x, float z)
    {
        Vector3 pos;
        //meshRenderer.enabled = false;

        if (Application.isPlaying)
        {
            WaitCallback callback = new WaitCallback(delegate
            {
                try
                {
                    pos = new Vector3(x, 0, z);
                    meshData = TerrainGenerator.Generate(datas, pos);
                    actionsToDo.Enqueue(PutMeshValues);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            });

            ThreadPool.QueueUserWorkItem(callback);
        }
        else
        {
            pos = transform.position;
            meshData = TerrainGenerator.Generate(datas, pos);
            PutMeshValues();
        }
    }

    private void PutMeshValues()
    {
        Mesh temp = GetComponent<MeshFilter>().sharedMesh;
        if (temp)
            DestroyImmediate(temp);

        mesh = new Mesh();
        mesh.vertices = meshData.vertices;
        mesh.normals = meshData.normals;
        mesh.triangles = meshData.triangles;
        mesh.colors = meshData.colors;

        //meshRenderer.enabled = true;
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Update()
    {
        // Execute actions in main thread
        if (actionsToDo.Count > 0)
        {
            actionsToDo.Dequeue()();
        }
    }

    private void OnDrawGizmos()
    {
        if (mesh && showVertices)
        {
            foreach (Vector3 v in mesh.vertices)
                Gizmos.DrawSphere(v, 0.1f);
        }
    }

    private void OnValidate()
    {
        if (datas)
        {
            datas.DiscardEventReferences();
            datas.updateEvent += delegate
            {
                Generate(0, 0);
            };
        }
    }
}
