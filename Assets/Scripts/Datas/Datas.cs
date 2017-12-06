using System;
using UnityEngine;

[CreateAssetMenu()]
public class Datas : ScriptableObject
{
    public bool autoUpdate;

    public TerrainVars tVars;
    public NoiseVars nVars;

    public event EventHandler updateEvent;

    public void Update()
    {
        if (updateEvent != null)
            updateEvent(this, null);
    }

    public void DiscardEventReferences()
    {
        updateEvent = null;
    }
}