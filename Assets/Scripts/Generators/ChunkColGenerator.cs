using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ChunkColGenerator : MonoBehaviour
{
    public Datas datas;
    private Mesh mesh;

    private MeshData meshData;
    private Queue<Action> actionsToDo
        = new Queue<Action>();

    private void Start() { }

    public void Generate(
        float x, float z)
    {
        Vector3 pos = new Vector3(x, 0, z);

        WaitCallback callback = new WaitCallback(delegate
        {
            try
            {
                meshData = ColliderGenerator.Generate(datas, pos);
                actionsToDo.Enqueue(PutMeshValues);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        });

        ThreadPool.QueueUserWorkItem(callback);
    }

    private void PutMeshValues()
    {
        Mesh temp = GetComponent<MeshCollider>().sharedMesh;
        if (temp)
            Destroy(temp);

        mesh = new Mesh();
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.triangles;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void Update()
    {
        // Execute actions in main thread
        if (actionsToDo.Count > 0)
        {
            actionsToDo.Dequeue()();
        }
    }

}
