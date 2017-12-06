using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Datas))]
public class DatasEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Datas datas = target as Datas;
        if (DrawDefaultInspector()
            && datas.autoUpdate
            || GUILayout.Button("Update"))
        {
            datas.Update();
        }
    }

}
