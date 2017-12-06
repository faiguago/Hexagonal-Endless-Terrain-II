using System;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrainManager : MonoBehaviour
{
    public Transform viewer;
    public Datas datas;

    private Hex hexPos;

    private Dictionary<Hex, GameObject> chunksDictionary =
        new Dictionary<Hex, GameObject>();

    private Queue<GameObject> chunksPool
        = new Queue<GameObject>();

    private Dictionary<Hex, GameObject> collidersDictionary =
        new Dictionary<Hex, GameObject>();

    private Queue<GameObject> collidersPool
        = new Queue<GameObject>();

    private Vector2 currentPos, oldPos;

    private void Awake() { }

    private void Start()
    {
        hexPos =
            Hex.PixelToHex(
                new Vector2(viewer.position.x, viewer.position.z),
                datas.tVars.WorldSize);

        UpdateTerrain();
    }

    private void Update()
    {
        currentPos =
            new Vector2(viewer.position.x,
            viewer.position.z);

        if (Vector2.Distance(currentPos, oldPos)
            >= datas.tVars.WorldSize / 4)
        {
            oldPos = currentPos;

            Hex temp = Hex.PixelToHex(
                currentPos, datas.tVars.WorldSize);
            if (hexPos != temp)
            {
                hexPos = temp;
                UpdateTerrain();
            }
        }
    }

    private void UpdateTerrain()
    {
        UpdateDictionaries();
        UpdateChunks();
        UpdateColliders();
    }

    private void UpdateDictionaries()
    {
        // Update chunks dictionary
        List<Hex> keys = new List<Hex>();
        foreach (var h in chunksDictionary.Keys)
        {
            if (Hex.Distance(hexPos, h) > datas.tVars.GridSize)
            {
                keys.Add(h);
                chunksDictionary[h].SetActive(false);
                chunksPool.Enqueue(chunksDictionary[h]);
            }
        }

        for (int i = 0; i < keys.Count; i++) //>
        {
            chunksDictionary.Remove(keys[i]);
        }

        // Update colliders dictionary
        keys.Clear();
        foreach (var h in collidersDictionary.Keys)
        {
            if (Hex.Distance(hexPos, h) > 1)
            {
                keys.Add(h);
                collidersDictionary[h].SetActive(false);
                collidersPool.Enqueue(collidersDictionary[h]);
            }
        }

        for (int i = 0; i < keys.Count; i++)
        {
            collidersDictionary.Remove(keys[i]);
        }
    }

    private void UpdateChunks()
    {
        Hex[] ring = Hex.Spiral(hexPos, datas.tVars.GridSize);
        for (int i = 0; i < ring.Length; i++)
        {
            CreateHex(ref ring[i]);
        }

        if (!chunksDictionary.ContainsKey(hexPos))
        {
            CreateHex(ref hexPos);
        }
    }

    private void CreateHex(ref Hex pos)
    {
        if (!chunksDictionary.ContainsKey(pos))
        {
            if (chunksPool.Count == 0)
            {
                NewHex(ref pos);
            }
            else
            {
                ReuseHex(ref pos);
            }
        }
    }

    private void NewHex(ref Hex pos)
    {
        GameObject go = new GameObject(pos.ToString(),
                    typeof(ChunkGenerator));

        Vector2 center = Hex.HexToPixel(pos, datas.tVars.WorldSize);

        go.transform.position = new Vector3(center.x, 0, center.y);
        go.transform.SetParent(transform, false);

        ChunkGenerator map = go.GetComponent<ChunkGenerator>();
        map.datas = datas;
        map.Generate(center.x, center.y);

        go.GetComponent<MeshRenderer>().sharedMaterial =
            map.datas.tVars.material;
        chunksDictionary.Add(pos, go);
    }

    private void ReuseHex(ref Hex pos)
    {
        GameObject go = chunksPool.Dequeue();
        go.SetActive(true);

        Vector2 center = Hex.HexToPixel(pos, datas.tVars.WorldSize);

        go.transform.position = new Vector3(center.x, 0, center.y);

        ChunkGenerator map = go.GetComponent<ChunkGenerator>();
        map.Generate(center.x, center.y);

        chunksDictionary.Add(pos, go);
    }

    private void UpdateColliders()
    {
        Hex[] ring = Hex.Spiral(hexPos, 1);
        for (int i = 0; i < ring.Length; i++) //>
        {
            CreateCollider(ref ring[i]);
        }

        if (!collidersDictionary.ContainsKey(hexPos))
        {
            CreateCollider(ref hexPos);
        }
    }

    private void CreateCollider(ref Hex pos)
    {
        if (!collidersDictionary.ContainsKey(pos))
        {
            if (collidersPool.Count == 0)
            {
                NewCollider(ref pos);
            }
            else
            {
                ReuseCollider(ref pos);
            }
        }
    }

    private void NewCollider(ref Hex pos)
    {
        GameObject go = new GameObject("Col: " + pos.ToString(),
                       typeof(ChunkColGenerator));

        Vector2 center = Hex.HexToPixel(pos, datas.tVars.WorldSize);

        go.transform.position = new Vector3(center.x, 0, center.y);
        go.transform.SetParent(transform, false);

        ChunkColGenerator map = go.GetComponent<ChunkColGenerator>();
        map.datas = datas;
        map.Generate(center.x, center.y);

        collidersDictionary.Add(pos, go);
    }

    private void ReuseCollider(ref Hex pos)
    {
        GameObject go = collidersPool.Dequeue();
        go.SetActive(true);

        Vector2 center = Hex.HexToPixel(pos, datas.tVars.WorldSize);

        go.transform.position = new Vector3(center.x, 0, center.y);

        ChunkColGenerator map = go.GetComponent<ChunkColGenerator>();
        map.Generate(center.x, center.y);

        collidersDictionary.Add(pos, go);
    }

}
